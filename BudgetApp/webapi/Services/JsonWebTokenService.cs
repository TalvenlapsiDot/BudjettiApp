using Back_End.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_End.Services
{
    public class JsonWebTokenService
    {
        // Needs improvements, but this will do for now
        public static string GenerateTokenUser(User User, IConfiguration _configuration)
        {
            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();

            var TokenLifeTime = _configuration.GetSection("Jwt:TokenLifeTime").Value!;
            TimeSpan Hours = TimeSpan.Parse(TokenLifeTime);

            byte[] Key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Secret").Value!);

            List<Claim> Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, User.Username),
                //new Claim(ClaimTypes.Role, "User")
            };

            SecurityTokenDescriptor tokenDescriptiptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.UtcNow.Add(Hours),
                Issuer = "https://localhost:7123/", // Invalid?
                Audience = "https://localhost:7123/", // Invalid?
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken Token = Handler.CreateToken(tokenDescriptiptor);

            string Jwt = Handler.WriteToken(Token);

            return Jwt;
        }

    }
}