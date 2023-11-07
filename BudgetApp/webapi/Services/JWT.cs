using Back_End.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_End.Services
{
    public class JWT
    {
        public static string GenerateTokenUser(User User, IConfiguration _configuration)
        {
            List<Claim> Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, User.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            SymmetricSecurityKey? Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Secret").Value!));
            SigningCredentials? Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);

            var Expires = DateTime.Now.AddHours(2);

            JwtSecurityToken Token = new JwtSecurityToken(
                claims: Claims,
                expires: Expires,
                signingCredentials: Credentials

                );

            string? Result = new JwtSecurityTokenHandler().WriteToken(Token);

            return Result;
        }

    }
}