using BlogAppWasm.Models;

namespace BlogAppWasm.Services;

public interface IAuthenticationService
{
    Task<RegisterResponse> RegisterUser(UserRegister userToRegister);
    Task<AuthenticationResponse> Login(UserAuthentication userFromAuthentication);
    Task Logout();
}