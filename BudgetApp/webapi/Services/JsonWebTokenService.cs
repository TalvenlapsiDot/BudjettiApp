using Back_End.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_End.Services
{
    public class JsonWebTokenService
    {
        public static string GenerateToken(User User, IConfiguration _configuration)
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


        // Work in progress
        public static ClaimsPrincipal GetClaims(string Token, IConfiguration _configuration)
        {
            if (Token.Contains("Bearer"))
            {
                int index = Token.IndexOf(" ") + 1;
                Token = Token.Substring(index);
            }

            SecurityToken ValidatedToken;

            // Token lifetime validation
            TokenValidationParameters ValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)),
            };


            ClaimsPrincipal Principal = new JwtSecurityTokenHandler().ValidateToken(Token, ValidationParameters, out ValidatedToken);

            return Principal;
        }

        public static string GetUniqueName(HttpRequest Request, IConfiguration _configuration)
        {
            var token = Request.Headers["Authorization"];
            var Token = token[0];

            if (Token == null)
            {
                return "Request.Headers[\"Authorization\"] was empty or null!";
            }
            else if (Token.Contains("Bearer"))
            {
                int Index = Token.IndexOf(" ") + 1;
                Token = Token.Substring(Index);
            }

            var JwtResults = new JwtSecurityTokenHandler().ReadJwtToken(Token);
            string unique_name = JwtResults.Claims.SingleOrDefault(c => c.Type == "unique_name").ToString();

            if (unique_name.Contains("unique_name"))
            {
                int Index = unique_name.IndexOf(" ") + 1;
                unique_name = unique_name.Substring(Index);
            }

            return unique_name;
        }

    }
}