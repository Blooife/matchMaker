using Bogus;
using Profile.Application.DTOs.User.Request;
using Profile.Domain.Models;

namespace Profile.Tests.Fakers;

public static class UserFakers
{
    public static Faker<User> CreateUser()
    {
        return new Faker<User>()
            .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Email, f => f.Internet.Email());
    }
    
    public static Faker<CreateUserDto> CreateCreateUserDto()
    {
        return new Faker<CreateUserDto>()
            .RuleFor(p => p.Id, f => f.Random.Guid().ToString())
            .RuleFor(p => p.Email, f => f.Internet.Email());
    }
}