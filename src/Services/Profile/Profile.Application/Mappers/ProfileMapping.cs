using Profile.Application.DTOs.Profile.Request;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.DTOs.User.Request;
using Profile.Domain.Models;
using Shared.Messages.Authentication;
using Shared.Messages.Profile;

namespace Profile.Application.Mappers;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<UserProfile, ProfileResponseDto>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal))
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages))
            .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests))
            .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.ProfileEducations))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)).ReverseMap();
        
        CreateMap<CreateProfileDto, UserProfile>();
        CreateMap<UpdateProfileDto, UserProfile>();

        CreateMap<UserCreatedMessage, CreateUserDto>();
        CreateMap<UserDeletedMessage, DeleteUserDto>();

        CreateMap<UserProfile, ProfileCreatedMessage>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images[0].ImageUrl : null));
        CreateMap<UserProfile, ProfileDeletedMessage>();
        CreateMap<UserProfile, ProfileUpdatedMessage>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images[0].ImageUrl : null));
    }
}