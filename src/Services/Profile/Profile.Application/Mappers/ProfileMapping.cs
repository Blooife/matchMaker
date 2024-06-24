using Profile.Application.DTOs.Profile.Request;
using Profile.Application.DTOs.Profile.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<UserProfile, ProfileResponseDto>();
        CreateMap<CreateProfileDto, UserProfile>();
        CreateMap<UpdateProfileDto, UserProfile>();
    }
}