using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using Tryitter.Models;

namespace Tryitter.Validations
{
    public static class UserValidation
    {
        public static bool IsValidUser(User? user)
        {
            return user is not null;
        }
        public static bool IsAdminUser(User? user)
        {
            return user is not null;
        }

        public static bool IsAdminUser(StringValues token)
        {
            var claims = GetTokenClaims(token);

            return bool.Parse(claims.Claims.ElementAt(1).Value.ToString());
        }

        private static JwtSecurityToken GetTokenClaims(StringValues token)
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

        //https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        public static bool IsValidEmail(string email)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(email, expression) && Regex.Replace(email, expression, string.Empty).Length == 0)
            {
                return true;
            }
            return false;
        }
    }
}
