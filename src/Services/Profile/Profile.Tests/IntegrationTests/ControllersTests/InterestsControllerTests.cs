using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Profile.Application.DTOs.Interest.Request;
using Profile.Application.DTOs.Interest.Response;
using Profile.Tests.Fakers;
using Profile.Tests.IntegrationTests.Fixtures;
using Shared.Models;

namespace Profile.Tests.IntegrationTests.ControllersTests;

public class InterestsControllerTests(CustomWebApplicationFactory factory) : ClassFixture(factory)
{
    private const string BaseUrl = "/api/interests";
    
    [Fact]
    public async Task GetAllCities_NoCitiesInDb_ReturnsEmptyCollection()
    {
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<InterestResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllCities_ReturnsCities()
    {
        await AddEntitiesToDbAsync(InterestFakers.CreateInterest().Generate(5));
        
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<InterestResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task GetInterestById_InterestExists_ReturnsInterest()
    {
        var interest = InterestFakers.CreateInterest().Generate();
        await AddEntitiesToDbAsync(interest);
        
        var response = await Client.GetAsync($"{BaseUrl}/{interest.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<InterestResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(interest.Id);
    }
    
    [Fact]
    public async Task GetInterestById_InterestNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddInterestToProfile_ProfileNotExists_ReturnsNotFound()
    {
        var dto = InterestFakers.CreateAddInterestToProfileDto().Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddInterestToProfile_InterestNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var dto = InterestFakers.CreateAddInterestToProfileDto().Clone().RuleFor(dto=>dto.ProfileId, profile.Id).Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddInterestToProfile_ValidData_ReturnsProfilesInterests()
    {
        var profile = await AddValidProfileToDbAsync();
        var interest = InterestFakers.CreateInterest().Generate();
        var dto = new AddInterestToProfileDto()
        {
            InterestId = interest.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(interest);
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<InterestResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task AddInterestToProfile_NotValidData_ReturnsBadRequest()
    {
        var dto = new AddInterestToProfileDto()
        {
            ProfileId = "",
            InterestId = 0
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("ValidationError");
    }
    
    [Fact]
    public async Task AddInterestToProfile_ProfileAlreadyContainsInterest_ReturnsConflict()
    {
        var profile = await AddValidProfileToDbAsync();
        var interest = InterestFakers.CreateInterest().Generate();
        profile.Interests.Add(interest);
        var dto = new AddInterestToProfileDto()
        {
            InterestId = interest.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(interest);
        await UpdateDbEntityAsync(profile);
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("AlreadyContainsError");
    }
    
    [Fact]
    public async Task AddInterestToProfile_ExceededMaxAmount_ReturnsInternalServerError()
    {
        var profile = await AddValidProfileToDbAsync();
        var interests = InterestFakers.CreateInterest().Generate(6);
        profile.Interests.AddRange(interests);

        await AddEntitiesToDbAsync(interests);
        await UpdateDbEntityAsync(profile);
        
        var interest = InterestFakers.CreateInterest().Generate();
        await AddEntitiesToDbAsync(interest);
        var dto = new AddInterestToProfileDto()
        {
            InterestId = interest.Id,
            ProfileId = profile.Id
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("Failure");
    }
    
    [Fact]
    public async Task RemoveInterestFromProfile_ProfileNotExists_ReturnsNotFound()
    {
        var dto = InterestFakers.CreateRemoveInterestFromProfileDto().Generate();
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/profile")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task RemoveInterestFromProfile_InterestNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var dto = InterestFakers.CreateRemoveInterestFromProfileDto().Clone().RuleFor(dto=>dto.ProfileId, profile.Id).Generate();
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/profile")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task RemoveInterestFromProfile_ValidData_ReturnsProfilesInterests()
    {
        var profile = await AddValidProfileToDbAsync();
        var interest = InterestFakers.CreateInterest().Generate();
        profile.Interests.Add(interest);
        var dto = new RemoveInterestFromProfileDto()
        {
            InterestId = interest.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(interest);
        await UpdateDbEntityAsync(profile);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/profile")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<InterestResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task RemoveInterestFromProfile_ProfileNotContainsInterest_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var interest = InterestFakers.CreateInterest().Generate();
        var dto = new RemoveInterestFromProfileDto()
        {
            InterestId = interest.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(interest);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/profile")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotContainsError");
    }
    
    [Fact]
    public async Task RemoveInterestFromProfile_NotValidData_ReturnsBadRequest()
    {
        var dto = new RemoveInterestFromProfileDto()
        {
            ProfileId = "",
            InterestId = 0
        };
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/profile")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("ValidationError");
    }
    
}