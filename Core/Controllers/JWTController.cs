using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public JWTController(IConfiguration Config, IUserService userService)
        {
            this.Config = Config;
            this.userService = userService;
        }

       
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginJwt login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);
            
            if(user != null && user.Result.IsAccess)
            {
                var tokenString = BuildToken(user.Result);
                response = Ok(new { token = tokenString });
            }

            return response;
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
