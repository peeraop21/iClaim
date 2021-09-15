using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class JWTController : Controller
    {
        private readonly IConfiguration Config;

        public JWTController(IConfiguration Config)
        {
            this.Config = Config;
        }

       
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]User login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);
            
            if(user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(Config["Jwt:Expires"]));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("RVP", "Learn JWT BY RVP"),
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

        private User Authenticate(User login)
        {
            var mockData = new User{
            Name = "Nior",
            Email = "peeran@rvp.co.th"
            };
            
            if(mockData.Name.Equals(login.Name) && mockData.Email.Equals(login.Email))
            {
                return mockData;
            }
            
            
            return null ;
        }


    }
}
