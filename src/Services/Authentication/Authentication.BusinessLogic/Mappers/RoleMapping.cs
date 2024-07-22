using Authentication.BusinessLogic.DTOs.Response;
using Authentication.DataLayer.Models;
using AutoMapper;

namespace Authentication.BusinessLogic.Mappers;

public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<string, Role>()
            .ForMember(dest => dest.Name, act=> act.MapFrom(src => src))
            .ForMember(dest => dest.NormalizedName, act=> act.MapFrom(src => src.ToUpper()));
        
        CreateMap<Role, RoleResponseDto>();
    }
}