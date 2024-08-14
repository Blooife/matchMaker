using System.Net;
using System.Net.Http.Json;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.Tests.UnitTests.Fakers;
using Bogus;
using FluentAssertions;
using Shared.Constants;
using Shared.Models;

namespace Authentication.Tests.IntegrationTests.ControllersTests;

public class RolesControllerTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    private readonly Faker<UserRequestDto> _userRequestDtoFaker = TestDataGenerator.CreateUserRequestDto();
    private const string BaseUrl = "/api/roles";
    
    [Fact]
    public async Task GetAllRoles_WithProperAuthorization_ReturnsExpectedRoles()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<RoleResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().NotBeEmpty();
        responseDto.Should().HaveCountGreaterThan(0);
    }
    
    [Fact]
    public async Task GetAllRoles_WithoutAuthorization_ReturnsUnauthorized()
    {
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AssignRole_ValidData_ReturnsOk()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        var requestDto = new AssignRoleRequestDto()
        {
            Email = user.Email!,
            Role = Roles.Admin,
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/assignment", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task AssignRole_UserNotExists_ReturnsNotFound()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var requestDto = new AssignRoleRequestDto()
        {
            Email = "notExistingEmail@gmail.com",
            Role = Roles.Admin,
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/assignment", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFound");
    }
    
    [Fact]
    public async Task AssignRole_RoleNotExists_ReturnsBadRequest()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        var requestDto = new AssignRoleRequestDto()
        {
            Email = user.Email!,
            Role = "notExistingRole",
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/assignment", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();

        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("AssignRoleError");
    }
    
    [Fact]
    public async Task AssignRole_UserAlreadyInRole_ReturnsBadRequest()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        await AssignUserRoleAsync(user, Roles.User);
        var requestDto = new AssignRoleRequestDto()
        {
            Email = user.Email!,
            Role = Roles.User,
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/assignment", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();

        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("AssignRoleError");
    }
    
    [Fact]
    public async Task RemoveFromRole_ValidData_ReturnsOk()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        await AssignUserRoleAsync(user, Roles.User);
        await AssignUserRoleAsync(user, Roles.Admin);
        var requestDto = new AssignRoleRequestDto()
        {
            Email = user.Email!,
            Role = Roles.Admin,
        };
        
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/removal", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task RemoveFromRole_UserNotExists_ReturnsNotFound()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var requestDto = new AssignRoleRequestDto()
        {
            Email = "notExistingEmail@gmail.com",
            Role = Roles.Admin,
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/removal", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFound");
    }
    
    [Fact]
    public async Task RemoveFromRole_RoleNotExists_ReturnsBadRequest()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        var requestDto = new AssignRoleRequestDto()
        {
            Email = user.Email!,
            Role = "notExistingRole",
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/removal", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();

        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("RemoveRoleError");
    }
    
    [Fact]
    public async Task RemoveFromRole_UserIsNotInRole_ReturnsBadRequest()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        await AssignUserRoleAsync(user, Roles.Admin);
        var requestDto = new AssignRoleRequestDto()
        {
            Email = user.Email!,
            Role = Roles.User,
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/removal", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();

        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("RemoveRoleError");
    }
    
    [Fact]
    public async Task RemoveFromRole_UserHasOnlyOneRole_ReturnsBadRequest()
    {
        AddJwtTokenToHeader(AdminJwtToken);
        var user = await AddUserToDbAsync(_userRequestDtoFaker.Generate());
        await AssignUserRoleAsync(user, Roles.Admin);
        var requestDto = new AssignRoleRequestDto()
        {
            Email = user.Email!,
            Role = Roles.Admin,
        };
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/removal", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();

        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("RemoveRoleError");
    }
}