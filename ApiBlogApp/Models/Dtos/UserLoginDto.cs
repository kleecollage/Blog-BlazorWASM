using System.ComponentModel.DataAnnotations;

namespace ApiBlogApp.Models.Dtos;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email is required")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}