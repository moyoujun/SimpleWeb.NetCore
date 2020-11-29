using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleWeb.NetCore.Auth
{
    internal class AuthorizationBaseOnRolesHandler : AuthorizationHandler<RoleRequirement>
    {
        private UserManager<IdentityUser> _userManager;

        public AuthorizationBaseOnRolesHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            // Bail out if the office number claim isn't present
            var user =  GetCurrentUser(context);
      
            if (user == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            foreach (var role in requirement.Roles)
            {
                var check = _userManager.IsInRoleAsync(user, role);
                check.Wait();

                if (check.Result)
                {
                    context.Succeed(requirement);
                }
            }

            context.Fail();
            return Task.CompletedTask;
        }

        protected IdentityUser GetCurrentUser(AuthorizationHandlerContext context)
        {
            var user = context.User;
            var identity = user.Identity as ClaimsIdentity;
            var userEmail = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return null;
            }

            var t =  _userManager.FindByEmailAsync(userEmail);
            t.Wait();
            return t.Result;
        }
    }

    // A custom authorization requirement which requires office number to be below a certain value
    internal class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(string[] roles)
        {
            Roles = roles;
        }

        public string[] Roles { get; private set; }
    }
}
