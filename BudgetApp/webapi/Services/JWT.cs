using Back_End.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_End.Services
{
    public class JWT
    {
        private static readonly TimeSpan time = TimeSpan.FromHours(1);
        public static string GenerateTokenUser(User User, IConfiguration _configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Secret").Value!);

            List<Claim> Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, User.Username),
                //new Claim(ClaimTypes.Role, "User")
            };

            var tokenDescriptiptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.UtcNow.Add(time),
                Issuer = "https://localhost:7123/",
                Audience = "https://localhost:7123/",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptiptor);

            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

    }
}