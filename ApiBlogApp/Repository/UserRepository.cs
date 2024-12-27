using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiBlogApp.Data;
using ApiBlogApp.Models;
using ApiBlogApp.Models.Dtos;
using ApiBlogApp.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using XSystem.Security.Cryptography;

namespace ApiBlogApp.Repository;

public class UserRepository: IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _secretWord;

    public UserRepository(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _secretWord = config.GetValue<string>("ApiSettings:Secret");;
    }
    public ICollection<User> GetAllUsers()
    {
        return _context.Users.OrderBy(u => u.Id).ToList();
    }

    public User GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public bool IsUniqueUser(string username)
    {
        var userDb = _context.Users.FirstOrDefault(u => u.Username == username);
        if (userDb == null) return true;

        return false;
    }
    
    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        var passwordEncrypted = ObtainMd5(userLoginDto.Password);
        var user = _context.Users.FirstOrDefault(
            u => u.Username.ToLower() == userLoginDto.Username.ToLower()
            && u.Password == passwordEncrypted
        );
        // INVALID LOGIN ATTEMPT
        if (user == null)
        {
            return new UserLoginResponseDto()
            {
                Token = "",
                User = null
            };
        }
        // USER LOGIN OK
        var handlerToken = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretWord);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                // new Claim(ClaimTypes.Role, user.Role),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = handlerToken.CreateToken(tokenDescriptor);
        UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto
        {
            Token = handlerToken.WriteToken(token),
            User = user
        };
        
        return userLoginResponseDto;
    }

    public async Task<User> Register(UserRegisterDto userRegisterDto)
    {
        var passwordEncypted = ObtainMd5(userRegisterDto.Password);

        User user = new User
        {
            Username = userRegisterDto.Username,
            Name = userRegisterDto.Name,
            Email = userRegisterDto.Email,
            Password = passwordEncypted,
        };
        
        _context.Users.Add(user);
        user.Password = passwordEncypted;
        await _context.SaveChangesAsync();
        
        return user;
    }
    
    // METHOD TO ENCRYPT PASSWORD WITH MD5 (called on Access and Register)
    public static string ObtainMd5(string value)
    {
        MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        byte[] data = Encoding.UTF8.GetBytes(value);
        data = x.ComputeHash(data);
        string resp = "";
        for (int i = 0; i < data.Length; i++)
            resp += data[i].ToString("x2").ToLower();
        
        return resp;
    }
}
















