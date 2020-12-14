using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleWeb.NetCore.Services;

namespace SimpleWeb.NetCore.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
            ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<Dto.JsonResponseModel<JsonWebTokenModal>> Login(LoginDto model)
        {
            var checkService = new SignatureCheckService(_configuration);
            if (!checkService.Verify(Request.Headers, model))
            {
                return new Dto.JsonResponseModel<JsonWebTokenModal>
                {
                    Data = null,
                    Msg = "Signature error",
                    Status = Dto.JsonResponseStatus.AuthFail
                };
            }


            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            var result = await _signInManager.PasswordSignInAsync(appUser, model.Password, false, false);

            if (result.Succeeded)
            {
                if (!appUser.EmailConfirmed)
                {
                    return new Dto.JsonResponseModel<JsonWebTokenModal>
                    {
                        Data = null,
                        Msg = "Account is not actived",
                        Status = Dto.JsonResponseStatus.AccountError
                    };
                }


                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                return new Dto.JsonResponseModel<JsonWebTokenModal> { 
                    Msg = "Login success!",
                    Data = new JsonWebTokenModal() 
                    {
                        Token= GenerateJwtToken(model.Email, appUser),
                        UserName = appUser.UserName
                    },
                    Status = Dto.JsonResponseStatus.Success
                };
            }

            //HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;

            return new Dto.JsonResponseModel<JsonWebTokenModal>
            {
                Data = null,
                Msg = "Login Failed, password or useremail wrong",
                Status = Dto.JsonResponseStatus.RequestError
            };
        }

        [HttpPost("Register")]
        public async Task<Dto.JsonResponseModel> Register(RegisterDto model)
        {
            var checkService = new SignatureCheckService(_configuration);
            if (!checkService.Verify(Request.Headers, model))
            {
                return new Dto.JsonResponseModel<Dto.JsonResponseModel>
                {
                    Data = null,
                    Msg = "Signature error",
                    Status = Dto.JsonResponseStatus.AuthFail
                };
            }

            var user = new IdentityUser
            {
                UserName = model.Name,
                Email = model.Email
            };

            if (_userManager.Users.FirstOrDefault(o => o.Email == user.Email) != null )
            {
                return new Dto.JsonResponseModel
                {   Msg= "email depulicated",
                    Status = Dto.JsonResponseStatus.RequestError
                };
            }

            if (_userManager.Users.FirstOrDefault(o => o.UserName == user.UserName) != null)
            {
                return new Dto.JsonResponseModel
                {
                    Msg = "name depulicated",
                    Status = Dto.JsonResponseStatus.RequestError
                };
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                // force user to confirm email, generate token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // generate url
                var callbackUrl = Url.ActionLink("Confirm", "Account", new RouteValueDictionary { { "id", user.Id }, { "token", token } }, "http");
                var emailService = new EmailService(_configuration);

                emailService.SendConfirmEmail(user, callbackUrl);

                return new Dto.JsonResponseModel
                {
                    Msg = $"Need confirm the email address",
                    Status = Dto.JsonResponseStatus.Success
                };
            }
            return new Dto.JsonResponseModel
            {
                Msg = $"Register failed, {result.Errors.First().Description}",
                Status = Dto.JsonResponseStatus.RequestError
            };
        }



        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> Confirm(string id, string token)
        {
            string url = _configuration["ClientHomePage"];


            if (id == null || token == null)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Redirect($"{url}/Error/404");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;

                return Redirect($"{url}/Error/404");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                return Redirect(url);
            }

            return Redirect($"{url}/Error/404");
        }

        [Authorize]
        [HttpGet("is_auth")]
        public bool IsAuthorizedToken()
        {
            return true;
        }

        private string GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            public override string ToString()
            {
                return Email + "." + Password;
            }

        }

        public class ConfirmModel
        {
            public string UserID { get; set; }

            public string Token { get; set; }
        }

        public class RegisterDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }

            public override string ToString()
            {
                return Email + "." + Password;
            }
        }

        public class JsonWebTokenModal
        {
            public string Token { get; set; }

            public string UserName { get; set; }
        }
    }
}
