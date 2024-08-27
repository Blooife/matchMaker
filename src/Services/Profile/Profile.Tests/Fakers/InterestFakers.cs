using Bogus;
using Profile.Application.DTOs.Interest.Request;
using Profile.Application.DTOs.Interest.Response;
using Profile.Domain.Models;

namespace Profile.Tests.Fakers;

public static class InterestFakers
{
    public static Faker<InterestResponseDto> CreateInterestResponseDto()
    {
        return new Faker<InterestResponseDto>()
            .RuleFor(i => i.Id, f => f.Random.Int())
            .RuleFor(i => i.Name, f => f.Lorem.Word());
    }
    
    public static Faker<Interest> CreateInterest()
    {
        return new Faker<Interest>()
            .RuleFor(i => i.Id, f => f.Random.Int())
            .RuleFor(i => i.Name, f => f.Lorem.Word());
    }
    
    public static Faker<AddInterestToProfileDto> CreateAddInterestToProfileDto()
    {
        return new Faker<AddInterestToProfileDto>()
            .RuleFor(i => i.InterestId, f => f.Random.Int())
            .RuleFor(i => i.ProfileId, f => f.Random.Guid().ToString());
    }
    
    public static Faker<RemoveInterestFromProfileDto> CreateRemoveInterestFromProfileDto()
    {
        return new Faker<RemoveInterestFromProfileDto>()
            .RuleFor(i => i.InterestId, f => f.Random.Int())
            .RuleFor(i => i.ProfileId, f => f.Random.Guid().ToString());
    }
}