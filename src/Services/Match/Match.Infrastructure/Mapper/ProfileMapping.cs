using Match.Application.DTOs.Profile.Response;

namespace Match.Infrastructure.Mapper;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<Protos.Profile, ProfileResponseDto>()
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDate)))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal))
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages))
            .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests))
            .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education));

        CreateMap<Protos.City, CityResponseDto>();
        CreateMap<Protos.Country, CountryResponseDto>();
        CreateMap<Protos.Goal, GoalResponseDto>();
        CreateMap<Protos.Language, LanguageResponseDto>();
        CreateMap<Protos.Interest, InterestResponseDto>();
        CreateMap<Protos.ProfileEducation, ProfileEducationResponseDto>();
        CreateMap<Protos.Image, ImageResponseDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url));
    }
}