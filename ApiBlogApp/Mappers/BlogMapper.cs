using ApiBlogApp.Models;
using ApiBlogApp.Models.Dtos;
using AutoMapper;

namespace ApiBlogApp.Mappers;

public class BlogMapper: Profile
{
    public BlogMapper()
    {
        CreateMap<Post, PostDto>().ReverseMap();
        CreateMap<Post, PostCreateDto>().ReverseMap();
        CreateMap<Post, PostUpdateDto>().ReverseMap();
        
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserLoginResponseDto>().ReverseMap();
        CreateMap<User, UserRegisterDto>().ReverseMap();
    }
}