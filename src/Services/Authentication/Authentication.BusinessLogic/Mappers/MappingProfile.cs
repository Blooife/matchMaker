using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.Request;
using DataLayer.Models;

namespace BusinessLogic.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRequestDto, User>()
            .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.Email))
            .ForMember(dest => dest.NormalizedEmail, act => act.MapFrom(src => src.Email.ToUpper()));
        CreateMap<User, UserDto>().ReverseMap();
    }
}