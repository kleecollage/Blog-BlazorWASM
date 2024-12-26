using System.ComponentModel.DataAnnotations;

namespace ApiBlogApp.Models.Dtos;

public class PostCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Tags are required")]
    public string Tags { get; set; }
    
    public string PathImage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}