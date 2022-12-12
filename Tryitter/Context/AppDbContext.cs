using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
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
    public User? GetUser(StringValues token)
    {

        var claims = GetTokenClaims(token);
        var email = claims.Claims.First().Value;

        return Users!.AsNoTracking().FirstOrDefault(user => user.Email == email);
    }

    public JwtSecurityToken GetTokenClaims(StringValues token)
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
