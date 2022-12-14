using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tryitter.Models;

public class Post
{
    [Key]
    [JsonIgnore]

    public int PostId { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string? ImageUrl { get; set; }
    [JsonIgnore]

    public DateTime DataPost { get; set; }
    [JsonIgnore]

    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
