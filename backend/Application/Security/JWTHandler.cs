
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Security
{
    public interface IJWTHandler
    {
        string CreateToken(string username, bool isAdmin);
    }
    
    public class JWTHandler : IJWTHandler
    {
        private readonly AuthenticationSettings config;
        public JWTHandler(IOptions<AuthenticationSettings> config)
        {
            this.config = config.Value;
        }

        public string CreateToken(string username, bool isAdmin)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.AuthKey));

            // generate signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>()
            {
                new Claim("username", username),
                new Claim("isAdmin", isAdmin.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(config.TokenExpirationDays),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}