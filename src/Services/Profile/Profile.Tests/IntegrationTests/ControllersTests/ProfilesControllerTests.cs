using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Profile.Application.DTOs.Profile.Response;
using Profile.Tests.Fakers;
using Profile.Tests.IntegrationTests.Fixtures;
using Shared.Models;

namespace Profile.Tests.IntegrationTests.ControllersTests;

public class ProfilesControllerTests(CustomWebApplicationFactory factory) : ClassFixture(factory)
{
    private const string BaseUrl = "/api/profiles";
    
    [Fact]
    public async Task CreateProfile_ValidData_ReturnsProfile()
    {
        var user = UserFakers.CreateUser().Generate();
        var country = CountryFakers.CreateCountry().Generate();
        var city = CityFakers.CreateCity().Clone().RuleFor(c => c.CountryId, country.Id).Generate();
        var profileDto = ProfileFakers.CreateCreateProfileDto()
            .RuleFor(p=>p.CityId, city.Id)
            .RuleFor(p=>p.UserId, user.Id)
            .Generate();

        await AddEntitiesToDbAsync(user);
        await AddEntitiesToDbAsync(country);
        await AddEntitiesToDbAsync(city);

        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}", profileDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<ProfileResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Name.Should().Be(profileDto.Name);
    }
    
    [Fact]
    public async Task CreateProfile_NotValidData_ReturnsBadRequest()
    {
        var profileDto = ProfileFakers.CreateCreateProfileDto()
            .RuleFor(p=>p.Name, "")
            .Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}", profileDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("ValidationError");
    }
    
    [Fact]
    public async Task CreateProfile_RelatedEntitiesNotExistInDb_ReturnsBadRequest()
    {
        var profileDto = ProfileFakers.CreateCreateProfileDto()
            .Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}", profileDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("DatabaseUpdateError");
    }
    
    [Fact]
    public async Task UpdateProfile_ValidData_ReturnsProfile()
    {
        var profile = await AddValidProfileToDbAsync();
        var profileDto = ProfileFakers.CreateUpdateProfileDto().Clone()
            .RuleFor(dto=>dto.Id, profile.Id)
            .RuleFor(dto=>dto.CityId, profile.CityId)
            .RuleFor(dto=>dto.UserId, profile.UserId)
            .Generate();
        profileDto.Name = "NewName";
        
        var response = await Client.PutAsJsonAsync($"{BaseUrl}", profileDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<ProfileResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Name.Should().Be(profileDto.Name);
    }
    
    [Fact]
    public async Task UpdateProfile_ProfileNotExists_ReturnsNotFound()
    {
        var profileDto = ProfileFakers.CreateUpdateProfileDto().Generate();
        
        var response = await Client.PutAsJsonAsync($"{BaseUrl}", profileDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task UpdateProfile_NotValidData_ReturnsBadRequest()
    {
        var profile = await AddValidProfileToDbAsync();
        var profileDto = ProfileFakers.CreateUpdateProfileDto().Clone()
            .RuleFor(dto=>dto.Id, profile.Id)
            .Generate();
        profileDto.Name = "";
        
        var response = await Client.PutAsJsonAsync($"{BaseUrl}", profileDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("ValidationError");
    }
    
    [Fact]
    public async Task UpdateProfile_RelatedEntitiesNotExistInDb_ReturnsBadRequest()
    {
        var profile = await AddValidProfileToDbAsync();
        var profileDto = ProfileFakers.CreateUpdateProfileDto().Clone()
            .RuleFor(dto=>dto.Id, profile.Id)
            .Generate();
        
        var response = await Client.PutAsJsonAsync($"{BaseUrl}", profileDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("DatabaseUpdateError");
    }
    
    [Fact]
    public async Task GetProfileById_ProfileExists_ReturnsProfile()
    {
        var profile = await AddValidProfileToDbAsync();
        
        var response = await Client.GetAsync($"{BaseUrl}/{profile.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<ProfileResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(profile.Id);
    }
    
    [Fact]
    public async Task GetProfileById_ProfileNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        
        var response = await Client.GetAsync($"{BaseUrl}/{profile.Id}notExistingId");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task GetProfileByUserId_ProfileExists_ReturnsProfile()
    {
        var profile = await AddValidProfileToDbAsync();
        
        var response = await Client.GetAsync($"{BaseUrl}/user/{profile.UserId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<ProfileResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(profile.Id);
    }
    
    [Fact]
    public async Task GetProfileByUserId_ProfileNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        
        var response = await Client.GetAsync($"{BaseUrl}/user/{profile.UserId}notExistingId");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
}