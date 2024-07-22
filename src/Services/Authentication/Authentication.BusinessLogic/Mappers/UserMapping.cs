using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.DataLayer.Models;
using AutoMapper;
using Shared.Messages.Authentication;

namespace Authentication.BusinessLogic.Mappers;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<UserRequestDto, User>()
            .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.Email))
            .ForMember(dest => dest.NormalizedEmail, act => act.MapFrom(src => src.Email.ToUpper()));
        
        CreateMap<User, UserResponseDto>().ReverseMap();

        CreateMap<User, UserCreatedMessage>();
        
        CreateMap<User, UserDeletedMessage>();
    }
}