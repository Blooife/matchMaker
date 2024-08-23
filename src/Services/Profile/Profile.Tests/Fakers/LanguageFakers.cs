using Bogus;
using Profile.Application.DTOs.Language.Request;
using Profile.Application.DTOs.Language.Response;
using Profile.Domain.Models;

namespace Profile.Tests.Fakers;

public class LanguageFakers
{
    public static Faker<LanguageResponseDto> CreateLanguageResponseDto()
    {
        return new Faker<LanguageResponseDto>()
            .RuleFor(l => l.Id, f => f.Random.Int())
            .RuleFor(l => l.Name, f => f.Lorem.Word());
    }
    
    public static Faker<Language> CreateLanguage()
    {
        return new Faker<Language>()
            .RuleFor(l => l.Id, f => f.Random.Int())
            .RuleFor(l => l.Name, f => f.Lorem.Word());
    }
    
    public static Faker<AddLanguageToProfileDto> CreateAddLanguageToProfileDto()
    {
        return new Faker<AddLanguageToProfileDto>()
            .RuleFor(i => i.LanguageId, f => f.Random.Int())
            .RuleFor(i => i.ProfileId, f => f.Random.Guid().ToString());
    }
    
    public static Faker<RemoveLanguageFromProfileDto> CreateRemoveLanguageFromProfileDto()
    {
        return new Faker<RemoveLanguageFromProfileDto>()
            .RuleFor(i => i.LanguageId, f => f.Random.Int())
            .RuleFor(i => i.ProfileId, f => f.Random.Guid().ToString());
    }
}