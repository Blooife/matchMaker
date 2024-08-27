using Profile.Application.DTOs.User.Request;
using Profile.Application.DTOs.User.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class UserMapping : AutoMapper.Profile
{
    public UserMapping()
    {
        CreateMap<User, UserResponseDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
    }
}