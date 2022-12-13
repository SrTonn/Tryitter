using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace Tryitter.Helper
{
    public static class ReadJWTToken
    {
        public static JwtSecurityToken GetTokenClaims(StringValues token)
        {
            var jwt = token.ToString();

            if (jwt.Contains("Bearer"))
            {
                jwt = jwt.Replace("Bearer", "").Trim();
            }

            var handler = new JwtSecurityTokenHandler();

            var finalToken = handler.ReadJwtToken(jwt);

            return finalToken;
        }
    }
}
