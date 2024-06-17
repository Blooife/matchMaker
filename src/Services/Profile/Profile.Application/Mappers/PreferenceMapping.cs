using Profile.Application.DTOs.Preference.Request;
using Profile.Application.DTOs.Preference.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class PreferenceMapping : AutoMapper.Profile
{
    public PreferenceMapping()
    {
        CreateMap<Preference, PreferenceResponseDto>();
        CreateMap<UpdatePreferenceDto, Preference>();
    }
}