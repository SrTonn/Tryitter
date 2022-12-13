using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using Tryitter.Models;

namespace Tryitter.Context;

public class AppDbContext : DbContext
{
    public DbSet<Post>? Posts { get; set; }
    public DbSet<User>? Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public User? GetUser(int id)
    {
        return Users!.AsNoTracking().FirstOrDefault(user => user.UserId == id);
    }
    public User? GetUser(string email)
    {
        return Users!.AsNoTracking().FirstOrDefault(user => user.Email == email);
    }
}
