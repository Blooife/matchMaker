using Match.Application.DTOs.Profile.Response;

namespace Match.Infrastructure.Mapper;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
            CreateMap<Protos.Profile, FullProfileResponseDto>()
                .ForMember(dest => dest.Id, 
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, 
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.LastName, 
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Bio, 
                    opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.Height, 
                    opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.BirthDate, 
                    opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.ShowAge, 
                    opt => opt.MapFrom(src => src.ShowAge))
                .ForMember(dest => dest.AgeFrom, 
                    opt => opt.MapFrom(src => src.AgeFrom))
                .ForMember(dest => dest.AgeTo, 
                    opt => opt.MapFrom(src => src.AgeTo))
                .ForMember(dest => dest.Gender, 
                    opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.PreferredGender, 
                    opt => opt.MapFrom(src => src.PreferredGender))
                .ForMember(dest => dest.MaxDistance, 
                    opt => opt.MapFrom(src => src.MaxDistance))
                .ForMember(dest => dest.Country, 
                    opt => opt.MapFrom(src => src.City.Country.Name))
                .ForMember(dest => dest.City, 
                    opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.Languages, 
                    opt => opt.MapFrom(src => src.Languages.Select(language => language.Name)))
                .ForMember(dest => dest.Interests, 
                    opt => opt.MapFrom(src => src.Interests.Select(interest => interest.Name)))
                .ForMember(dest => dest.Goal, 
                    opt => opt.MapFrom(src => src.Goal.Name));
    }
}