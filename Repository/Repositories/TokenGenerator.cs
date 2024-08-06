using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(PremierLeagueAccount account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
         {
             new Claim(ClaimTypes.NameIdentifier, account.AccId.ToString()),
             new Claim(ClaimTypes.Name, account.EmailAddress!),
             new Claim(ClaimTypes.Role, account.Role!.ToString()),
         };

            var securityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(5),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
