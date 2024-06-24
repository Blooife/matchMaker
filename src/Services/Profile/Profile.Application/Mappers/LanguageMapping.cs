using Profile.Application.DTOs.Language.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class LanguageMapping : AutoMapper.Profile
{
    public LanguageMapping()
    {
        CreateMap<Language, LanguageResponseDto>();
    }
}