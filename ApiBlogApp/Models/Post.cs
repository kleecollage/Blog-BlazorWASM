using System.ComponentModel.DataAnnotations;

namespace ApiBlogApp.Models;

public class Post
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Tags { get; set; }
    
    public string? PathImage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime UpdatedAt { get; set; }
}