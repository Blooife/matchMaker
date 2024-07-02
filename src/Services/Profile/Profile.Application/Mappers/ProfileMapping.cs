using Profile.Application.DTOs.Profile.Request;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.DTOs.User.Request;
using Profile.Domain.Models;
using Shared.Messages.Authentication;

namespace Profile.Application.Mappers;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<UserProfile, ProfileResponseDto>();
        CreateMap<CreateProfileDto, UserProfile>();
        CreateMap<UpdateProfileDto, UserProfile>();

        CreateMap<UserCreatedMessage, CreateUserDto>();
        CreateMap<UserDeletedMessage, DeleteUserDto>();
    }
}