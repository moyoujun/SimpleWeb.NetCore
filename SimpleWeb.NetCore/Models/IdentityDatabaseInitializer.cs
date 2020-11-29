using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWeb.NetCore.Models
{
    public class IdentityDatabaseInitializer
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;

        public IdentityDatabaseInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public void InitialUserAndRoles()
        {
            const string SUPER_USER = "superuser";
            const string ADMINISTRATOR = "administrator";
            const string NORMA_USER = "normaluser";

            var roleList = new string[]
            {
                SUPER_USER, ADMINISTRATOR, NORMA_USER
            };

            foreach (var role in roleList)
            {
                if (!_roleManager.Roles.Any(o => o.Name == role))
                {
                    _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var superuser = new IdentityUser()
            { 
                Email = "moyoujun00@qq.com",
                UserName = "test01"
            };

            _userManager.CreateAsync(superuser, "moyoujun00.QQ.com");
            _userManager.AddToRoleAsync(superuser, SUPER_USER);
        }

    }
}
