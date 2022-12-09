using Microsoft.EntityFrameworkCore;
using Tryitter.Models;

namespace Tryitter.Validations
{
    public static class UserValidation
    {
        public static bool IsValid(User? user)
        {
            return user is not null;
        }
    }
}
