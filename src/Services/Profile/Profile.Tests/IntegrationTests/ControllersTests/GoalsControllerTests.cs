using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Profile.Application.DTOs.Goal.Response;
using Profile.Tests.Fakers;
using Profile.Tests.IntegrationTests.Fixtures;
using Shared.Models;

namespace Profile.Tests.IntegrationTests.ControllersTests;

public class GoalsControllerTests(CustomWebApplicationFactory factory) : ClassFixture(factory)
{
    private const string BaseUrl = "/api/goals";
    
    [Fact]
    public async Task GetAllCities_NoCitiesInDb_ReturnsEmptyCollection()
    {
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<GoalResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllCities_ReturnsCities()
    {
        await AddEntitiesToDbAsync(GoalFakers.CreateGoal().Generate(5));
        
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<GoalResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task GetGoalById_GoalExists_ReturnsGoal()
    {
        var goal = GoalFakers.CreateGoal().Generate();
        await AddEntitiesToDbAsync(goal);
        
        var response = await Client.GetAsync($"{BaseUrl}/{goal.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<GoalResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(goal.Id);
    }
    
    [Fact]
    public async Task GetGoalById_GoalNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
}