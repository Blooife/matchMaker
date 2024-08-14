using System.Net;
using System.Net.Http.Json;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.DataLayer.Models;
using Authentication.Tests.UnitTests.Fakers;
using Bogus;
using FluentAssertions;
using Shared.Models;

namespace Authentication.Tests.IntegrationTests.ControllersTests;

public class AuthControllerTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    private readonly Faker<UserRequestDto> _userRequestDtoFaker = TestDataGenerator.CreateUserRequestDto();
    private const string BaseUrl = "/api/auth";

    [Fact]
    public async Task Register_ValidUser_ReturnsOk()
    {
        var requestDto = _userRequestDtoFaker.Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/register", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Register_NotValidUser_ReturnsBadRequest()
    {
        var requestDto = _userRequestDtoFaker.Clone().RuleFor(u=>u.Email, "").Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/register", requestDto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();

        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("ValidationError");
    }
    
    [Fact]
    public async Task Register_UserAlreadyExists_ReturnsBadRequest()
    {
        var requestDto = _userRequestDtoFaker.Generate();
        await AddUserToDbAsync(requestDto);
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/register", requestDto);
        
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("RegisterError");
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsOk()
    {
        var requestDto = _userRequestDtoFaker.Generate();
        await AddUserToDbAsync(requestDto);
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/login", requestDto);
        
        var responseDto = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        
        responseDto.Should().NotBeNull();
        responseDto!.Email.Should().Be(requestDto.Email);
        responseDto.RefreshToken.Should().NotBeNull();
        responseDto.JwtToken.Should().NotBeNull();
    }

    [Fact]
    public async Task Login_NotExistingUser_ReturnsNotFound()
    {
        var requestDto = _userRequestDtoFaker.Generate();
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/login", requestDto);
        
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFound");
    }
    
    [Fact]
    public async Task Login_IncorrectPassword_ReturnsUnauthorized()
    {
        var requestDto = _userRequestDtoFaker.Generate();
        await AddUserToDbAsync(requestDto);
        requestDto.Password = "incorrectPassword";
        
        var response = await Client.PostAsJsonAsync($"{BaseUrl}/login", requestDto);
        
        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("LoginError");
    }
    
    [Fact]
    public async Task LoginThenRefresh_ValidToken_ReturnsOk()
    {
        var requestDto = _userRequestDtoFaker.Generate();
        await AddUserToDbAsync(requestDto);
        
        var loginResponse = await Client.PostAsJsonAsync($"{BaseUrl}/login", requestDto);
        
        var loginResponseDto = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

        var refreshDto = new RefreshRequestDto()
        {
            refreshToken = loginResponseDto!.RefreshToken!
        };
        var refreshResponse = await Client.PostAsJsonAsync($"{BaseUrl}/refresh", refreshDto);
        
        var refreshResponseDto = await refreshResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        
        refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        refreshResponseDto.Should().NotBeNull();
        refreshResponseDto!.JwtToken.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Refresh_ExpiredToken_ReturnsUnauthorized()
    {
        var requestDto = _userRequestDtoFaker.Generate();
        var user = new User()
        {
            Email = requestDto.Email,
            UserName = requestDto.Email.ToUpper(),
            RefreshToken = "refreshToken",
            RefreshTokenExpiredAt = DateTime.UtcNow.AddHours(-1)
        };
        await _userManager.CreateAsync(user, requestDto.Password);
        await _dbContext.SaveChangesAsync();

        var refreshDto = new RefreshRequestDto()
        {
            refreshToken = user.RefreshToken
        };
        
        var refreshResponse = await Client.PostAsJsonAsync($"{BaseUrl}/refresh", refreshDto);
        
        var refreshResponseDto = await refreshResponse.Content.ReadFromJsonAsync<ErrorDetails>();
        
        refreshResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        refreshResponseDto.Should().NotBeNull();
        refreshResponseDto!.ErrorType.Should().Be("LoginError");
    }
    
    [Fact]
    public async Task Refresh_NonExistingToken_ReturnsNotFound()
    {
        var refreshDto = new RefreshRequestDto()
        {
            refreshToken = "notExistingToken"
        };
        
        var refreshResponse = await Client.PostAsJsonAsync($"{BaseUrl}/refresh", refreshDto);
        
        var refreshResponseDto = await refreshResponse.Content.ReadFromJsonAsync<ErrorDetails>();
        
        refreshResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        refreshResponseDto.Should().NotBeNull();
        refreshResponseDto!.ErrorType.Should().Be("NotFound");
    }
}