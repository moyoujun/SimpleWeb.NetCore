using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWeb.NetCore.Controllers
{
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Policy = "SuperUserRoleOnly")]
        [HttpPost("grant")]
        public async Task<Dto.JsonResponseModel> GrantUserToAccess(string userEmail, string role)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);

            if (!roleExist)
            {
                return new Dto.JsonResponseModel()
                {
                    Msg = $"Role {role} not exist in this application.",
                    Status = Dto.JsonResponseStatus.RequestError
                };
            }
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return new Dto.JsonResponseModel()
                {
                    Msg = $"Cannot find user email {userEmail} in this application, need register the user first",
                    Status = Dto.JsonResponseStatus.RequestError
                };
            }


            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
            {
                return new Dto.JsonResponseModel()
                {
                    Msg = $"grant user with {role} success",
                    Status = Dto.JsonResponseStatus.Success
                };
            }

            return new Dto.JsonResponseModel()
            {
                Msg = string.Join(" | ", result.Errors),
                Status = Dto.JsonResponseStatus.ProcessFail
            };
        }
    }


}
