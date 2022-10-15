using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Transcore.Services.Membership.Authentication
{
    public class JwtTokenManager : ITokenManager
    {
        private readonly IConfiguration _appSettings;

        public JwtTokenManager(IConfiguration configuration)
        {
            this._appSettings = configuration;
        }

        public string GenerateToken()
        {
            throw new System.NotImplementedException();
        }

        public string GenerateToken(Dictionary<string, string> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(_appSettings.GetValue<string>("JwtKey"));

            var tokenDescriptor = new SecurityTokenDescriptor();

            tokenDescriptor.Subject = new ClaimsIdentity();

            foreach (var claim in claims)
                tokenDescriptor.Subject.AddClaim(new Claim(claim.Key, claim.Value));

            tokenDescriptor.Expires = System.DateTime.Now.AddMinutes(1);

            tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(jwtToken);


        }
    }
}
