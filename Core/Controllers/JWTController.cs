using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{

    [EnableCors("iClaim")]
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class JWTController : Controller
    {
        private readonly IConfiguration Config;
        private readonly IUserService userService;
        private readonly ILogger<JWTController> logger;

        public JWTController(IConfiguration Config, IUserService userService, ILogger<JWTController> logger)
        {
            this.Config = Config;
            this.userService = userService;
            this.logger = logger;
        }

       
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginJwt login)
        {
            try
            {
                IActionResult response = Unauthorized();
                var user = Authenticate(login);

                if (user != null && user.Result.IsAccess)
                {
                    var tokenString = BuildToken(user.Result);
                    response = Ok(new { token = tokenString });
                }

                return response;
            }
            catch (Exception ex)
            {
                string baseUrl = Config["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(login.UserId))
                {
                    logger.LogError(baseUrl + ", API: JwtCreateToken, User: {0}, Exception: {1}", login.UserId, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: JwtCreateToken, Exception: {0}", ex);
                    return StatusCode(500);
                }
            }
            
        }

        private string BuildToken(LoginJwt user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(Config["Jwt:Expires"]));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.SystemName),               
                new Claim("RVP", "iClaim"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                Config["Jwt:Issuer"],
                Config["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<LoginJwt> Authenticate(LoginJwt login)
        {

            var section = Config.GetSection($"Jwt:SystemLoginNames");
            var systemNames = section.Get<string[]>();
            string password = Config["Jwt:Password"];

            if (systemNames.Contains(login.SystemName) && password.Equals(login.Password))
            {
                bool isAccess = await userService.CheckUserAccessToken(login.SystemName, login.UserId);
                var result = new LoginJwt
                {
                    SystemName = login.SystemName,
                    Password = null,
                    UserId = login.UserId,
                    IsAccess = isAccess,
                };
                return result;
            }
            return null ;
        }


    }
}
