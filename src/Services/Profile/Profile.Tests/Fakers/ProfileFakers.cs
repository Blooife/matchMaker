using Bogus;
using Profile.Application.DTOs.Profile.Request;
using Profile.Application.DTOs.Profile.Response;
using Profile.Domain.Models;
using Shared.Constants;

namespace Profile.Tests.Fakers;

public static class ProfileFakers
{
    public static Faker<ProfileResponseDto> CreateProfileResponseDto()
    {
        return new Faker<ProfileResponseDto>()
            .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.BirthDate, DateTime.UtcNow.AddYears(-30))
            .RuleFor(p => p.Bio, f => f.Lorem.Sentence())
            .RuleFor(p => p.Height, f => f.Random.Int(150, 200))
            .RuleFor(p => p.ShowAge, f => f.Random.Bool())
            .RuleFor(p => p.AgeFrom, f => f.Random.Int(18, 65))
            .RuleFor(p => p.AgeTo, f => f.Random.Int(18, 65))
            .RuleFor(p => p.Gender, f => f.PickRandom(new []{Gender.Undefined, Gender.Male, Gender.Female}))
            .RuleFor(p => p.PreferredGender, f => f.PickRandom(new []{Gender.Undefined, Gender.Male, Gender.Female}))
            .RuleFor(p => p.MaxDistance, f => f.Random.Int(1, 100))
            .RuleFor(p => p.City, CityFakers.CreateCityResponseDto().Generate())
            .RuleFor(p => p.Country, CountryFakers.CreateCountryResponseDto().Generate())
            .RuleFor(p => p.Goal, GoalFakers.CreateGoalResponseDto().Generate())
            .RuleFor(p => p.UserId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Languages, LanguageFakers.CreateLanguageResponseDto().Generate(3))
            .RuleFor(p => p.Interests, InterestFakers.CreateInterestResponseDto().Generate(3))
            .RuleFor(p => p.Education, EducationFakers.CreateProfileEducationResponseDto().Generate(3))
            .RuleFor(p => p.Images, ImageFakers.CreateImageResponseDto().Generate(5));
    }
    
    public static Faker<UserProfile> CreateUserProfile()
    {
        return new Faker<UserProfile>()
            .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.BirthDate, DateTime.UtcNow.AddYears(-30))
            .RuleFor(p => p.Bio, f => f.Lorem.Sentence())
            .RuleFor(p => p.Height, f => f.Random.Int(150, 200))
            .RuleFor(p => p.ShowAge, f => f.Random.Bool())
            .RuleFor(p => p.AgeFrom, f => f.Random.Int(18, 65))
            .RuleFor(p => p.AgeTo, f => f.Random.Int(18, 65))
            .RuleFor(p => p.Gender, f => f.PickRandom(new []{Gender.Undefined, Gender.Male, Gender.Female}))
            .RuleFor(p => p.PreferredGender, f => f.PickRandom(new []{Gender.Undefined, Gender.Male, Gender.Female}))
            .RuleFor(p => p.MaxDistance, f => f.Random.Int(1, 100))
            .RuleFor(p => p.CityId, CityFakers.CreateCity().Generate().Id)
            .RuleFor(p => p.GoalId, GoalFakers.CreateGoal().Generate().Id)
            .RuleFor(p => p.UserId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Languages, LanguageFakers.CreateLanguage().Generate(3))
            .RuleFor(p => p.Interests, InterestFakers.CreateInterest().Generate(3))
            .RuleFor(p => p.ProfileEducations, EducationFakers.CreateProfileEducation().Generate(3))
            .RuleFor(p => p.Images, ImageFakers.CreateImage().Generate(5));
    }
    
    public static Faker<UserProfile> CreateEmptyUserProfile()
    {
        return new Faker<UserProfile>()
            .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.BirthDate, DateTime.UtcNow.AddYears(-30))
            .RuleFor(p => p.Bio, f => f.Lorem.Sentence())
            .RuleFor(p => p.Height, f => f.Random.Int(150, 200))
            .RuleFor(p => p.ShowAge, f => f.Random.Bool())
            .RuleFor(p => p.AgeFrom, f => f.Random.Int(18, 65))
            .RuleFor(p => p.AgeTo, f => f.Random.Int(18, 65))
            .RuleFor(p => p.Gender, f => f.PickRandom(new []{Gender.Undefined, Gender.Male, Gender.Female}))
            .RuleFor(p => p.PreferredGender, f => f.PickRandom(new []{Gender.Undefined, Gender.Male, Gender.Female}))
            .RuleFor(p => p.MaxDistance, f => f.Random.Int(1, 100))
            .RuleFor(p => p.CityId, CityFakers.CreateCity().Generate().Id)
            .RuleFor(p => p.UserId, f => f.Random.Guid().ToString());
    }
    
    public static Faker<CreateProfileDto> CreateCreateProfileDto()
    {
        return new Faker<CreateProfileDto>()
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.BirthDate, DateTime.UtcNow.AddYears(-30))
            .RuleFor(p => p.Bio, f => f.Lorem.Sentence())
            .RuleFor(p => p.Height, f => f.Random.Int(150, 200))
            .RuleFor(p => p.ShowAge, f => f.Random.Bool())
            .RuleFor(p => p.AgeFrom, f => f.Random.Int(18, 30))
            .RuleFor(p => p.AgeTo, f => f.Random.Int(40, 70))
            .RuleFor(p => p.Gender, f => f.PickRandom(new[] { Gender.Undefined, Gender.Male, Gender.Female }))
            .RuleFor(p => p.PreferredGender, f => f.PickRandom(new[] { Gender.Undefined, Gender.Male, Gender.Female }))
            .RuleFor(p => p.MaxDistance, f => f.Random.Int(1, 100))
            .RuleFor(p => p.CityId, CityFakers.CreateCity().Generate().Id)
            .RuleFor(p => p.UserId, f => f.Random.Guid().ToString());
    }
    
    public static Faker<UpdateProfileDto> CreateUpdateProfileDto()
    {
        return new Faker<UpdateProfileDto>()
            .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.BirthDate, DateTime.UtcNow.AddYears(-30))
            .RuleFor(p => p.Bio, f => f.Lorem.Sentence())
            .RuleFor(p => p.Height, f => f.Random.Int(150, 200))
            .RuleFor(p => p.ShowAge, f => f.Random.Bool())
            .RuleFor(p => p.AgeFrom, f => f.Random.Int(18, 30))
            .RuleFor(p => p.AgeTo, f => f.Random.Int(40, 70))
            .RuleFor(p => p.Gender, f => f.PickRandom(new[] { Gender.Undefined, Gender.Male, Gender.Female }))
            .RuleFor(p => p.PreferredGender, f => f.PickRandom(new[] { Gender.Undefined, Gender.Male, Gender.Female }))
            .RuleFor(p => p.MaxDistance, f => f.Random.Int(1, 100))
            .RuleFor(p => p.CityId, CityFakers.CreateCity().Generate().Id)
            .RuleFor(p => p.UserId, f => f.Random.Guid().ToString());
    }
}