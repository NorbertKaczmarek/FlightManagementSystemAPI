using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightManagementSystem.Controllers
{
    [Route("token")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Pozwala utworzyć token przykładowy potrzebny do autoryzacji przy niektórych endpointach.
        /// </summary>
        [HttpGet]
        public IActionResult GetToken()
        {
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, "user-id"),
                new Claim(ClaimTypes.Name, "Test Name"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<String>("Authentication:JwtIssuer"),
                audience: _configuration.GetValue<String>("Authentication:JwtIssuer"),
                claims: claims,
            expires: DateTime.UtcNow.AddDays(60),
            notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<String>("Authentication:JwtKey"))),
                    SecurityAlgorithms.HmacSha256
                )
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwtToken);
        }
    }
}
