using System.Net;
using ApiBlogApp.Models;
using ApiBlogApp.Models.Dtos;
using ApiBlogApp.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlogApp.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IUserRepository userRepo, IMapper mapper) : ControllerBase
{
    private readonly ApiResponse _apiResponse = new();

    // POST: User registration [host/api/users/register]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        bool isUniqueUsername = userRepo.IsUniqueUser(userRegisterDto.Username);
        if (!isUniqueUsername)
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("Username already taken");
            return BadRequest(_apiResponse);
        }
        
        var user = await userRepo.Register(userRegisterDto);
        if (user == null)
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("Registration failed");
            return BadRequest(_apiResponse);
        }
        
        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.IsSuccess = true;
        return Ok(_apiResponse);
    }
    
    // POST: User log in [host/api/users/login]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var loginResponse = await userRepo.Login(userLoginDto);
        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("Invalid username or password");
            return BadRequest(_apiResponse);
        }
        
        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.IsSuccess = true;
        _apiResponse.Result = loginResponse;
        return Ok(_apiResponse);
    }

    // GET: Retrieves all registered users [hots/api/user]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllUsers()
    {
        var usersList = userRepo.GetAllUsers();
        var usersListDto = new List<UserDto>();

        foreach (var user in usersList)
        {
            usersListDto.Add(mapper.Map<UserDto>(user));
        }
        
        return Ok(usersListDto);
    }
    
    // GET: Given its Id, retrieves the user [hots/api/user]
    [HttpGet("{userId:int}" , Name = "GetUserById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetUserById(int userId)
    {
        var user = userRepo.GetUserById(userId);
        if (user == null) return NotFound();
        var userDto = mapper.Map<UserDto>(user);
        
        return Ok(userDto);
    }
    
}

















