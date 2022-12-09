using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Tryitter.Models;
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key] //Optional
    public int UserId { get; set; }
    [Required]
    [MinLength(3), MaxLength(10)]
    public string? Name { get; set; }
    [Required]
    [MinLength(3), MaxLength(30)]
    public string? Email { get; set; }
    [Required]
    [MinLength(6), MaxLength(20)]
    public string? Password { get; set; }
    public IEnumerable<Post> Posts { get; set; }

    public User()
    {
        Posts = new List<Post>();
    }
}
