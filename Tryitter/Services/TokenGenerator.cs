using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tryitter.Constants;
using Tryitter.DTOs;

namespace Tryitter.Services
{
    public class TokenGenerator
    {
        /// <summary>
        /// This function is to Generate Token 
        /// </summary>
        /// <returns>A string, the token JWT</returns>
        public string Generate(AuthDTO user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = AddClaims(user),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenConstants.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Expires = DateTime.Now.AddDays(1)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity AddClaims(AuthDTO user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("Email", user.Email!));

            return claims;
        }
    }
}
