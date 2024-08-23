using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Profile.Application.DTOs.Education.Request;
using Profile.Application.DTOs.Education.Response;
using Profile.Domain.Models;
using Profile.Tests.Fakers;
using Profile.Tests.IntegrationTests.Fixtures;
using Shared.Models;

namespace Profile.Tests.IntegrationTests.ControllersTests;

public class EducationsControllerTests(CustomWebApplicationFactory factory) : ClassFixture(factory)
{
    private const string BaseUrl = "/api/educations";
    
    [Fact]
    public async Task GetAllEducations_NoEducationsInDb_ReturnsEmptyCollection()
    {
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<EducationResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllEducations_ReturnsEducations()
    {
        await AddEntitiesToDbAsync(EducationFakers.CreateEducation().Generate(5));
        
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<EducationResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task GetEducationById_EducationExists_ReturnsEducation()
    {
        var education = EducationFakers.CreateEducation().Generate();
        await AddEntitiesToDbAsync(education);
        
        var response = await Client.GetAsync($"{BaseUrl}/{education.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<EducationResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(education.Id);
    }
    
    [Fact]
    public async Task GetEducationById_EducationNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddEducationToProfile_ProfileNotExists_ReturnsNotFound()
    {
        var dto = EducationFakers.CreateAddEducationToProfileDto().Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddEducationToProfile_EducationNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var dto = EducationFakers.CreateAddEducationToProfileDto().Clone().RuleFor(dto=>dto.ProfileId, profile.Id).Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddEducationToProfile_ValidData_ReturnsProfilesEducations()
    {
        var profile = await AddValidProfileToDbAsync();
        var education = EducationFakers.CreateEducation().Generate();
        var dto = new AddEducationToProfileDto()
        {
            EducationId = education.Id,
            ProfileId = profile.Id,
            Description = "Description"
        };

        await AddEntitiesToDbAsync(education);
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<EducationResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task AddEducationToProfile_NotValidData_ReturnsBadRequest()
    {
        var dto = new AddEducationToProfileDto()
        {
            ProfileId = "",
            EducationId = 0,
            Description = ""
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("ValidationError");
    }
    
    [Fact]
    public async Task AddEducationToProfile_ProfileAlreadyContainsEducation_ReturnsConflict()
    {
        var profile = await AddValidProfileToDbAsync();
        var education = EducationFakers.CreateEducation().Generate();
        var profileEducation = new ProfileEducation()
        {
            ProfileId = profile.Id,
            EducationId = education.Id,
            Description = "Description",
        };
        profile.ProfileEducations.Add(profileEducation);
        var dto = new AddEducationToProfileDto()
        {
            EducationId = education.Id,
            ProfileId = profile.Id,
            Description = "Description",
        };

        await AddEntitiesToDbAsync(education);
        await UpdateDbEntityAsync(profile);
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("AlreadyContainsError");
    }
    
    [Fact]
    public async Task RemoveEducationFromProfile_ProfileNotExists_ReturnsNotFound()
    {
        var dto = EducationFakers.CreateRemoveEducationFromProfileDto().Generate();
        
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
    public async Task RemoveEducationFromProfile_EducationNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var dto = EducationFakers.CreateRemoveEducationFromProfileDto().Clone().RuleFor(dto=>dto.ProfileId, profile.Id).Generate();
        
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
    public async Task RemoveEducationFromProfile_ValidData_ReturnsProfilesEducations()
    {
        var profile = await AddValidProfileToDbAsync();
        var education = EducationFakers.CreateEducation().Generate();
        var profileEducation = new ProfileEducation()
        {
            ProfileId = profile.Id,
            EducationId = education.Id,
            Description = "",
        };
        profile.ProfileEducations.Add(profileEducation);
        var dto = new RemoveEducationFromProfileDto()
        {
            EducationId = education.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(education);
        await UpdateDbEntityAsync(profile);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/profile")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<EducationResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task RemoveEducationFromProfile_ProfileNotContainsEducation_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var education = EducationFakers.CreateEducation().Generate();
        var dto = new RemoveEducationFromProfileDto()
        {
            EducationId = education.Id,
            ProfileId = profile.Id
        };

        await AddEntitiesToDbAsync(education);
        
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
    public async Task RemoveEducationFromProfile_NotValidData_ReturnsBadRequest()
    {
        var dto = new RemoveEducationFromProfileDto()
        {
            ProfileId = "",
            EducationId = 0
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
    
    [Fact]
    public async Task UpdateEducation_ProfileNotExists_ReturnsNotFound()
    {
        var dto = EducationFakers.CreateUpdateProfileEducationDto().Generate();
        
        var response = await Client.PutAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task UpdateEducation_EducationNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var dto = EducationFakers.CreateUpdateProfileEducationDto().Clone().RuleFor(dto=>dto.ProfileId, profile.Id).Generate();
        
        var response = await Client.PutAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task UpdateEducation_ValidData_ReturnsProfileEducation()
    {
        var profile = await AddValidProfileToDbAsync();
        var education = EducationFakers.CreateEducation().Generate();
        var profileEducation = new ProfileEducation()
        {
            ProfileId = profile.Id,
            EducationId = education.Id,
            Description = "",
        };
        profile.ProfileEducations.Add(profileEducation);
        var dto = new UpdateProfileEducationDto()
        {
            EducationId = education.Id,
            ProfileId = profile.Id,
            Description = "NewDescription"
        };

        await AddEntitiesToDbAsync(education);
        await UpdateDbEntityAsync(profile);
        
        var response = await Client.PutAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<ProfileEducationResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Description.Should().Be(dto.Description);
    }
    
    [Fact]
    public async Task UpdateEducation_ProfileNotContainsEducation_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var education = EducationFakers.CreateEducation().Generate();
        var dto = new UpdateProfileEducationDto()
        {
            EducationId = education.Id,
            ProfileId = profile.Id,
            Description = ""
        };

        await AddEntitiesToDbAsync(education);
        
        var response = await Client.PutAsJsonAsync($"{BaseUrl}/profile", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotContainsError");
    }
}