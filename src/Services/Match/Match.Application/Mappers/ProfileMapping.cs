using Match.Application.DTOs.Profile.Request;
using Match.Application.DTOs.Profile.Response;
using Match.Domain.Models;

namespace Match.Application.Mappers;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<CreateProfileDto, Profile>();
        CreateMap<UpdateProfileDto, Profile>();
        CreateMap<Profile, ProfileResponseDto>();
    }
}