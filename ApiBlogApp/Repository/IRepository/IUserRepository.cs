using ApiBlogApp.Models;
using ApiBlogApp.Models.Dtos;

namespace ApiBlogApp.Repository.IRepository;

public interface IUserRepository
{
    ICollection<User> GetAllUsers();
    User GetUserById(int id);
    bool IsUniqueUser(string username);
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    Task<User> Register(UserRegisterDto userRegisterDto);
}