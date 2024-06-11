using AutoMapper;
using Authentication.BusinessLogic.DTOs;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.DataLayer.Models;

namespace Authentication.BusinessLogic.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRequestDto, User>()
            .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.Email))
            .ForMember(dest => dest.NormalizedEmail, act => act.MapFrom(src => src.Email.ToUpper()));
        CreateMap<User, UserDto>().ReverseMap();
        
        CreateMap<string, Role>()
            .ForMember(dest => dest.Name, act=> act.MapFrom(src => src))
            .ForMember(dest => dest.NormalizedName, act=> act.MapFrom(src => src.ToUpper()));
        CreateMap<Role, RoleResponseDto>();
    }
}