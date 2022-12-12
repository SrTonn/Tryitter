using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Tryitter.Models;

namespace Tryitter.Validations
{
    public static class UserValidation
    {
        public static bool IsValid(User? user)
        {
            return user is not null;
        }

        //https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        public static bool IsValidEmail(string email)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(email, expression))
            {
                if (Regex.Replace(email, expression, string.Empty).Length == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
