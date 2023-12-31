﻿using Back_End.Models;
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
                Issuer = _configuration.GetSection("Jwt:Issuer").Value!, // Invalid?
                Audience = _configuration.GetSection("Jwt:Audience").Value!, // Invalid?
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken Token = Handler.CreateToken(tokenDescriptiptor);

            string Jwt = Handler.WriteToken(Token);

            return Jwt;
        }

        public static string GetUniqueName(HttpRequest Request, IConfiguration _configuration)
        {
            var Token = Request.Headers["Authorization"][0];

            if (Token == null)
            {
                return "Request.Headers[\"Authorization\"] was empty or null!";
            }
            else if (Token.ToUpper().Contains("BEARER"))
            {
                Token = Token.Substring(Token.IndexOf(" ") + 1);
            }

            var JwtResults = new JwtSecurityTokenHandler().ReadJwtToken(Token);
            var UniqueNameClaim = JwtResults.Claims.Single(c => c.Type == "unique_name");
            string? unique_name = UniqueNameClaim.ToString();

            if (unique_name != null && unique_name.Contains("unique_name"))
            {
                int Index = unique_name.IndexOf(" ") + 1;
                unique_name = unique_name.Substring(Index);

                return unique_name;
            }

            return "NameClaim null or empty";
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
                ValidateAudience = true,
                ValidateIssuer = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)),
            };

            ClaimsPrincipal Principal = new JwtSecurityTokenHandler().ValidateToken(Token, ValidationParameters, out ValidatedToken);

            return Principal;
        }

    }
}