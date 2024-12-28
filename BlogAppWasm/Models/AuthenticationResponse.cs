namespace BlogAppWasm.Models;

public class AuthenticationResponse
{
    public bool Success { get; set; }
    public string Token { get; set; }
    public User User { get; set; }
}