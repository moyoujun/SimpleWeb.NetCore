using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly EmailService _emailService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            EmailService emailService

            ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("Login")]
        public async Task<Dto.JsonResponseModel<JsonWebTokenModal>> Login(LoginDto model)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            var result = await _signInManager.PasswordSignInAsync(appUser, model.Password, false, false);

            if (result.Succeeded)
            {
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
            var user = new IdentityUser
            {
                UserName = model.Name,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                // force user to confirm email, generate token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // generate url
                var callbackUrl = Url.ActionLink("Confirm", "Account", new RouteValueDictionary { { "id", user.Id }, { "token",  token}, { "callbackUrl", model.ReturnUrl } }, "http");

                var sendResult = await _emailService.SendConfirmEmail(user, callbackUrl);

                if (sendResult)
                {
                    return new Dto.JsonResponseModel
                    {
                        Msg = $"Need confirm the email address",
                        Status = Dto.JsonResponseStatus.Success
                    };
                }

                else
                {
                    return new Dto.JsonResponseModel<string>
                    {
                        Msg = $"Need confirm the email address, but the email is not sending correctly",
                        Status = Dto.JsonResponseStatus.Success,
                    };
                }
            }

            HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return new Dto.JsonResponseModel
            {
                Msg = $"Register failed, {result.Errors}",
                Status = Dto.JsonResponseStatus.RequestError
            };
        }



        [HttpGet("ConfirmEmail")]
        public async Task<object> Confirm(string id, string token, string callbackUrl)
        {
            if (id == null || token == null)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

                //return new Dto.JsonResponseModel
                //{
                //    Msg = "arguments error",
                //    Status = Dto.JsonResponseStatus.RequestError
                //};

                return Redirect(callbackUrl + "/?success=false");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;

                //return new Dto.JsonResponseModel
                //{
                //    Msg = "user id not exist in current environment",
                //    Status = Dto.JsonResponseStatus.IDNotFound
                //};

                return Redirect(callbackUrl + "/?success=false");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                /*
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<div>Your email has been validated, go back to your login page click here" +
                    $"<div><a href=\"{callbackUrl}\">{callbackUrl}</a></div>" +
                    "</div>"
                };
                */

                return Redirect(callbackUrl+"/?success=true");
            }


            return Redirect(callbackUrl + "/?success=false");
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

            public string ReturnUrl { get; set; }
        }

        public class JsonWebTokenModal
        {
            public string Token { get; set; }

            public string UserName { get; set; }
        }
    }
}
