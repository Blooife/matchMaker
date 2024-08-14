using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.DataLayer.Models;
using Bogus;

namespace Authentication.Tests.UnitTests.Fakers;

public static class TestDataGenerator
{
    public static Faker<UserRequestDto> CreateUserRequestDto()
    {
        return new Faker<UserRequestDto>()
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password());
    }
    
    public static Faker<User> CreateUser()
    {
        return new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid().ToString())
            .RuleFor(u => u.Email, f => f.Internet.Email());
    }
    
    public static Faker<RoleResponseDto> CreateRoleResponseDto()
    {
        return new Faker<RoleResponseDto>()
            .RuleFor(r => r.Name, f => f.PickRandom(new[] { "Admin", "User", "Manager" }));
    }
}