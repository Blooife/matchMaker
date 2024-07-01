using Match.Application.DTOs.Profile.Response;
using Match.Infrastructure.Protos;
using Profile = AutoMapper.Profile;

namespace Match.Infrastructure.Mapper;

public class ProfileMapping : Profile
{
    public ProfileMapping()
    {
            CreateMap<GetProfileResponse, ProfileResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Profile.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Profile.Name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Profile.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Profile.BirthDate))
                .ForMember(dest => dest.AgeFrom, opt => opt.MapFrom(src => src.Profile.AgeFrom))
                .ForMember(dest => dest.AgeTo, opt => opt.MapFrom(src => src.Profile.AgeTo))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (Gender)src.Profile.Gender))
                .ForMember(dest => dest.PreferredGender, opt => opt.MapFrom(src => (Gender)src.Profile.PreferredGender))
                .ForMember(dest => dest.MaxDistance, opt => opt.MapFrom(src => src.Profile.MaxDistance))
                //.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Profile.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Profile.City))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Profile.Languages))
                .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Profile.Interests))
                .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Profile.Goal));
    }
}