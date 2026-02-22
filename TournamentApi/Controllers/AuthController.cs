using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TournamentApi.Controllers
{
    // för authenticationatt testa JWT-autentisering, inte en riktig login, så ingen validering av användare eller lösenord
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            // Här kan man validera användare, kör utan claims 
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "TestUser")
            };

            //var claims = new List<Claim>
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, "TestUser"),
            //    new Claim(ClaimTypes.Name, "TestUser"),
            //    new Claim(ClaimTypes.Role, "Admin") // om du vill kunna radera
            //};

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }



    }
}
