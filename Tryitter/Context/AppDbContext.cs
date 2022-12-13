using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Tryitter.Helper;
using Tryitter.Models;

namespace Tryitter.Context;

public class AppDbContext : DbContext
{
    public DbSet<Post>? Posts { get; set; }
    public DbSet<User>? Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public User? GetUser(int id)
    {
        return Users!.Include(p => p.Posts).FirstOrDefault(u => u.UserId == id);
    }
    public User? GetUser(string email)
    {
        return Users!.AsNoTracking().FirstOrDefault(user => user.Email == email);
    }    
    public User? GetUser(StringValues token)
    {
        var claims = ReadJWTToken.GetTokenClaims(token);

        var email = claims.Claims.ElementAt(0).Value;

        return Users!.AsNoTracking().FirstOrDefault(user => user.Email == email);
    }
}
