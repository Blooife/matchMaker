using Bogus;
using Profile.Application.DTOs.Education.Request;
using Profile.Application.DTOs.Education.Response;
using Profile.Domain.Models;

namespace Profile.Tests.Fakers;

public static class EducationFakers
{
    public static Faker<Education> CreateEducation()
    {
        return new Faker<Education>()
            .RuleFor(e => e.Id, f => f.Random.Int())
            .RuleFor(e => e.Name, f => f.Random.String2(new Random().Next(10, 20)));
    }

    public static Faker<EducationResponseDto> CreateEducationResponseDto()
    {
        return new Faker<EducationResponseDto>()
            .RuleFor(e => e.Id, f => f.Random.Int())
            .RuleFor(e => e.Name, f => f.Random.String2(new Random().Next(10, 20)));
    }
    
    public static Faker<ProfileEducationResponseDto> CreateProfileEducationResponseDto()
    {
        return new Faker<ProfileEducationResponseDto>()
            .RuleFor(p => p.ProfileId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.EducationId, f => f.Random.Int())
            .RuleFor(p => p.EducationName, f => f.Random.String2(new Random().Next(10, 20)))
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());
    }
    
    public static Faker<ProfileEducation> CreateProfileEducation()
    {
        return new Faker<ProfileEducation>()
            .RuleFor(p => p.ProfileId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.EducationId, f => f.Random.Int())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.Education, f => CreateEducation().Generate());
    }
    
    public static Faker<AddEducationToProfileDto> CreateAddEducationToProfileDto()
    {
        return new Faker<AddEducationToProfileDto>()
            .RuleFor(p => p.ProfileId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.EducationId, f => f.Random.Int())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());
    }

    public static Faker<UpdateProfileEducationDto> CreateUpdateProfileEducationDto()
    {
        return new Faker<UpdateProfileEducationDto>()
            .RuleFor(p => p.ProfileId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.EducationId, f => f.Random.Int())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());
    }
    
    public static Faker<RemoveEducationFromProfileDto> CreateRemoveEducationFromProfileDto()
    {
        return new Faker<RemoveEducationFromProfileDto>()
            .RuleFor(p => p.ProfileId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.EducationId, f => f.Random.Int());
    }

}