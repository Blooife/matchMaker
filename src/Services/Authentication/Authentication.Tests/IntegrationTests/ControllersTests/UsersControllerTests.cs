using System.Net;
using System.Net.Http.Json;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.Tests.Fakers;
using Bogus;
using FluentAssertions;
using Shared.Models;

namespace Authentication.Tests.IntegrationTests.ControllersTests;

public class UsersControllerTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    private readonly Faker<UserRequestDto> _userRequestDtoFaker = TestDataGenerator.CreateUserRequestDto();
    private const string BaseUrl = "/api/users";
    
    [Fact]
    public async Task GetUserById_UserExists_ReturnsUser()
    {
        AddJwtTokenToHeader(UserJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        
        var response = await Client.GetAsync($"{BaseUrl}/{user.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<UserResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(user.Id);
    }
    
    [Fact]
    public async Task GetUserById_UserNotExists_ReturnsNotFound()
    {
        AddJwtTokenToHeader(UserJwtToken);
        
        var response = await Client.GetAsync($"{BaseUrl}/notExistingId");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFound");
    }
    
    [Fact]
    public async Task GetUserByEmail_UserExists_ReturnsUser()
    {
        AddJwtTokenToHeader(UserJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        
        var response = await Client.GetAsync($"{BaseUrl}/email/{user.Email}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<UserResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(user.Id);
    }
    
    [Fact]
    public async Task GetUserByEmail_UserNotExists_ReturnsNotFound()
    {
        AddJwtTokenToHeader(UserJwtToken);
        
        var response = await Client.GetAsync($"{BaseUrl}/email/notExistingEmail");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFound");
    }
    
    [Fact]
    public async Task DeleteUserById_UserExists_ReturnsOk()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        
        var response = await Client.DeleteAsync($"{BaseUrl}/{user.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteUserById_UserNotExists_ReturnsNotFound()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        
        var response = await Client.DeleteAsync($"{BaseUrl}/notExistingId");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFound");
    }
    
    [Fact]
    public async Task GetPagedUsers_EmptyCollection_ReturnsZeroUsers()
    {
        await factory.ResetDatabase();
        AddJwtTokenToHeader(AdminJwtToken);

        int pageNumber = 1;
        int pageSize = 10;
        var response = await Client.GetAsync($"{BaseUrl}/paginated?pageSize={pageSize}&pageNumber={pageNumber}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetPagedUsers_NotEmptyCollection_ReturnsUsers()
    {
        await factory.ResetDatabase();
        
        for (int i = 0; i < 5; i++)
        {
            await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        }
        
        AddJwtTokenToHeader(AdminJwtToken);

        int pageNumber = 1;
        int pageSize = 4;
        var response = await Client.GetAsync($"{BaseUrl}/paginated?pageSize={pageSize}&pageNumber={pageNumber}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().NotBeEmpty();
        responseDto.Should().HaveCount(4);
    }
    
    [Fact]
    public async Task GetPagedUsers_EmptyPage_ReturnsZeroUsers()
    {
        await factory.ResetDatabase();
        
        for (int i = 0; i < 5; i++)
        {
            await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        }
        
        AddJwtTokenToHeader(AdminJwtToken);

        int pageNumber = 2;
        int pageSize = 5;
        var response = await Client.GetAsync($"{BaseUrl}/paginated?pageSize={pageSize}&pageNumber={pageNumber}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
}