using Bogus;
using Profile.Application.DTOs.Goal.Response;
using Profile.Domain.Models;

namespace Profile.Tests.Fakers;

public static class GoalFakers
{
    public static Faker<GoalResponseDto> CreateGoalResponseDto()
    {
        return new Faker<GoalResponseDto>()
            .RuleFor(g => g.Id, f => f.Random.Int())
            .RuleFor(g => g.Name, f => f.Lorem.Sentence());
    }
    
    public static Faker<Goal> CreateGoal()
    {
        return new Faker<Goal>()
            .RuleFor(g => g.Id, f => f.Random.Int())
            .RuleFor(g => g.Name, f => f.Lorem.Sentence());
    }
}