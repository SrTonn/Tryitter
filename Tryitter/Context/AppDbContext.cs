using Microsoft.EntityFrameworkCore;
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
}
