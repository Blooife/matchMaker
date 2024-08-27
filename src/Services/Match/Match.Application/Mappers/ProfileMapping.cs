using Match.Application.DTOs.Profile.Request;
using Match.Domain.Models;
using Shared.Messages.Profile;

namespace Match.Application.Mappers;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<CreateProfileDto, Profile>();
        CreateMap<UpdateProfileDto, Profile>();

        CreateMap<ProfileCreatedMessage, CreateProfileDto>();
        CreateMap<ProfileUpdatedMessage, UpdateProfileDto>();
    }
}