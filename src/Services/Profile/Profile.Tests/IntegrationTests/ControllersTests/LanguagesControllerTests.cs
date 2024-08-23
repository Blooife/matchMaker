using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Profile.Application.DTOs.Language.Request;
using Profile.Application.DTOs.Language.Response;
using Profile.Tests.Fakers;
using Profile.Tests.IntegrationTests.Fixtures;
using Shared.Models;

namespace Profile.Tests.IntegrationTests.ControllersTests;

public class LanguagesControllerTests(CustomWebApplicationFactory factory) : ClassFixture(factory)
{
    private const string BaseUrl = "/api/languages";
    
    [Fact]
    public async Task GetAllCities_NoCitiesInDb_ReturnsEmptyCollection()
    {
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<LanguageResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllCities_ReturnsCities()
    {
        await AddEntitiesToDbAsync(LanguageFakers.CreateLanguage().Generate(5));
        
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<LanguageResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task GetLanguageById_LanguageExists_ReturnsLanguage()
    {
        var language = LanguageFakers.CreateLanguage().Generate();
        await AddEntitiesToDbAsync(language);
        
        var response = await Client.GetAsync($"{BaseUrl}/{language.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<LanguageResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(language.Id);
    }
    
    [Fact]
    public async Task GetLanguageById_LanguageNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddLanguageToProfile_ProfileNotExists_ReturnsNotFound()
    {
        var dto = LanguageFakers.CreateAddLanguageToProfileDto().Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddLanguageToProfile_LanguageNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var dto = LanguageFakers.CreateAddLanguageToProfileDto().Clone().RuleFor(dto=>dto.ProfileId, profile.Id).Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddLanguageToProfile_ValidData_ReturnsProfilesLanguages()
    {
        var profile = await AddValidProfileToDbAsync();
        var language = LanguageFakers.CreateLanguage().Generate();
        var dto = new AddLanguageToProfileDto()
        {
            LanguageId = language.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(language);
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<LanguageResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task AddLanguageToProfile_NotValidData_ReturnsBadRequest()
    {
        var dto = new AddLanguageToProfileDto()
        {
            ProfileId = "",
            LanguageId = 0
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("ValidationError");
    }
    
    [Fact]
    public async Task AddLanguageToProfile_ProfileAlreadyContainsLanguage_ReturnsConflict()
    {
        var profile = await AddValidProfileToDbAsync();
        var language = LanguageFakers.CreateLanguage().Generate();
        profile.Languages.Add(language);
        var dto = new AddLanguageToProfileDto()
        {
            LanguageId = language.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(language);
        await UpdateDbEntityAsync(profile);
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("AlreadyContainsError");
    }
    
    [Fact]
    public async Task RemoveLanguageFromProfile_ProfileNotExists_ReturnsNotFound()
    {
        var dto = LanguageFakers.CreateRemoveLanguageFromProfileDto().Generate();
        
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
    public async Task RemoveLanguageFromProfile_LanguageNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var dto = LanguageFakers.CreateRemoveLanguageFromProfileDto().Clone().RuleFor(dto=>dto.ProfileId, profile.Id).Generate();
        
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
    public async Task RemoveLanguageFromProfile_ValidData_ReturnsProfilesLanguages()
    {
        var profile = await AddValidProfileToDbAsync();
        var language = LanguageFakers.CreateLanguage().Generate();
        profile.Languages.Add(language);
        var dto = new RemoveLanguageFromProfileDto()
        {
            LanguageId = language.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(language);
        await UpdateDbEntityAsync(profile);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/profile")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<LanguageResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task RemoveLanguageFromProfile_ProfileNotContainsLanguage_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var language = LanguageFakers.CreateLanguage().Generate();
        var dto = new RemoveLanguageFromProfileDto()
        {
            LanguageId = language.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(language);
        
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
    public async Task RemoveLanguageFromProfile_NotValidData_ReturnsBadRequest()
    {
        var dto = new RemoveLanguageFromProfileDto()
        {
            ProfileId = "",
            LanguageId = 0
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