namespace Tryitter.Models;
//[Index(nameof(Email), IsUnique = true)]
public class OwnUser
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
