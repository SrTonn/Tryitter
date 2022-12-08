using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tryitter.Models;

public class Post
{
    [Key]
    public int PostId { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    [Required]
    [MaxLength(300)]
    public string? ImageUrl { get; set; }

    public DateTime DataPost { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
