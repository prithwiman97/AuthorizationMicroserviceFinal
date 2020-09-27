using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.JWT
{
    public class TokenGenerator
    {
        private IConfiguration Configuration { get; }
        public TokenGenerator(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Method for generating JWT
        /// </summary>
        /// <returns>Token in string format</returns>
        public string GenerateToken()
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenInfo:SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(30),
                issuer: Configuration["TokenInfo:Issuer"],
                audience: Configuration["TokenInfo:Issuer"],
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
