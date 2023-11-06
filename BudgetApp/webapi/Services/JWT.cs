using Back_End.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_End.Services
{
    public class JWT
    {
        public static string GenerateToken(User User, IConfiguration _configuration)
        {
            List<Claim> Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, User.Username)
            };

            SymmetricSecurityKey? Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            SigningCredentials? Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken Token = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.Now.AddDays(0.25),
                signingCredentials: Credentials
                );

            string? Result = new JwtSecurityTokenHandler().WriteToken(Token);

            return Result;
        }

        public static string ValidateToken()
        {

            return string.Empty;
        }

    }
}