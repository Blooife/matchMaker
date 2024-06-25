using Match.Application.DTOs.Profile.Request;
using Match.Application.DTOs.Profile.Response;
using Match.Domain.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Match.Application.Mappers;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<CreateProfileDto, Profile>();
        CreateMap<UpdateProfileDto, Profile>();
        CreateMap<Profile, ProfileResponseDto>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location != null ? (double?)src.Location.Coordinates.X : null))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location != null ? (double?)src.Location.Coordinates.Y : null));;
    }
}