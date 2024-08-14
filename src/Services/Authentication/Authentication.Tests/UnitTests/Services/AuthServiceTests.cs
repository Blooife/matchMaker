using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Producers;
using Authentication.BusinessLogic.Providers.Interfaces;
using Authentication.BusinessLogic.Services.Implementations;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using Authentication.Tests.UnitTests;
using Authentication.Tests.UnitTests.Fakers;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Messages.Authentication;
using Shared.Models;
using ValidationResult = FluentValidation.Results.ValidationResult;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly Mock<IJwtTokenProvider> _jwtTokenProviderMock;
    private readonly Mock<IRefreshTokenProvider> _refreshTokenProviderMock;
    private readonly Mock<IValidator<UserRequestDto>> _validatorMock;
    private readonly Mock<IProducerService> _producerServiceMock;
    private readonly AuthService _authService;
    private readonly Faker<UserRequestDto> _userRequestDtoFaker = TestDataGenerator.CreateUserRequestDto();

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<AuthService>>();
        _jwtTokenProviderMock = new Mock<IJwtTokenProvider>();
        _refreshTokenProviderMock = new Mock<IRefreshTokenProvider>();
        _validatorMock = new Mock<IValidator<UserRequestDto>>();
        _producerServiceMock = new Mock<IProducerService>();

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _jwtTokenProviderMock.Object,
            _refreshTokenProviderMock.Object,
            _validatorMock.Object,
            _producerServiceMock.Object
        );
    }

    [Fact]
    public async Task Register_ReturnsSuccess_WhenUserRegisteredSuccessfully()
    {
        var userRequestDto = _userRequestDtoFaker.Generate();
        var user = new User();
        var identityResultMock = IdentityResult.Success;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRequestDto>>(), default))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<User>(userRequestDto)).Returns(user);
        _userRepositoryMock.Setup(r => r.RegisterAsync(user, userRequestDto.Password))
                           .ReturnsAsync(identityResultMock);

        var result = await _authService.RegisterAsync(userRequestDto);

        result.Should().BeOfType(typeof(GeneralResponseDto));
        result.IsSuccess.Should().BeTrue();
        _userRepositoryMock.Verify(r => r.RegisterAsync(user, userRequestDto.Password), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<UserCreatedMessage>()), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Never());
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowRegisterException_WhenRegistrationFails()
    {
        var userRequestDto = _userRequestDtoFaker.Generate();
        var user = new User();
        var identityResultMock = IdentityResult.Failed(new IdentityError());

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRequestDto>>(), default))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<User>(userRequestDto)).Returns(user);
        _userRepositoryMock.Setup(r => r.RegisterAsync(user, userRequestDto.Password))
                           .ReturnsAsync(identityResultMock);

        Func<Task> act = async () => await _authService.RegisterAsync(userRequestDto);

        await act.Should().ThrowAsync<RegisterException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenLoginIsSuccessful()
    {
        var userRequestDto = _userRequestDtoFaker.Generate();
        var user = new User();
        var token = "valid_token";
        var roles = new[] { "User" };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRequestDto>>(), default))
            .ReturnsAsync(new ValidationResult());
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(userRequestDto.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.CheckPasswordAsync(user, userRequestDto.Password))
                           .ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.GetRolesAsync(user))
                           .ReturnsAsync(roles);
        _jwtTokenProviderMock.Setup(p => p.GenerateToken(user, roles))
                             .Returns(token);
        _refreshTokenProviderMock.Setup(p => p.GenerateRefreshToken())
                                 .Returns("new_refresh_token");
        _mapperMock.Setup(m => m.Map<LoginResponseDto>(user))
                   .Returns(new LoginResponseDto());

        var result = await _authService.LoginAsync(userRequestDto, CancellationToken.None);

        result.JwtToken.Should().Be(token);
        _userRepositoryMock.Verify(r => r.GetByEmailAsync(userRequestDto.Email, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.CheckPasswordAsync(user, userRequestDto.Password), Times.Once);
        _jwtTokenProviderMock.Verify(p => p.GenerateToken(user, roles), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateUserAsync(user), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Never());
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowLoginException_WhenUserNotFound()
    {
        var userRequestDto = _userRequestDtoFaker.Generate();
        
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRequestDto>>(), default))
            .ReturnsAsync(new ValidationResult());
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(userRequestDto.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(null as User);

        Func<Task> act = async () => await _authService.LoginAsync(userRequestDto, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowLoginException_WhenPasswordIsIncorrect()
    {
        var userRequestDto = _userRequestDtoFaker.Generate();
        var user = new User();

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRequestDto>>(), default))
            .ReturnsAsync(new ValidationResult());
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(userRequestDto.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.CheckPasswordAsync(user, userRequestDto.Password))
                           .ReturnsAsync(false);

        Func<Task> act = async () => await _authService.LoginAsync(userRequestDto, CancellationToken.None);

        await act.Should().ThrowAsync<LoginException>()
                 .WithMessage(ExceptionMessages.LoginFailed);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task RefreshTokenAsync_ReturnsNewToken_WhenRefreshTokenIsValid()
    {
        var refreshToken = "valid_refresh_token";
        var user = new User
        {
            RefreshToken = refreshToken,
            RefreshTokenExpiredAt = DateTime.Now.AddDays(1)
        };
        var newToken = "new_jwt_token";
        var newRefreshToken = "new_refresh_token";
        var identityResult = IdentityResult.Success;

        _userRepositoryMock.Setup(r => r.GetByRefreshTokenAsync(refreshToken, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _jwtTokenProviderMock.Setup(p => p.GenerateToken(user, It.IsAny<IEnumerable<string>>()))
                             .Returns(newToken);
        _refreshTokenProviderMock.Setup(p => p.GenerateRefreshToken())
                                 .Returns(newRefreshToken);
        _userRepositoryMock.Setup(r => r.UpdateUserAsync(It.Is<User>(u => u.RefreshToken == newRefreshToken)))
                           .ReturnsAsync(identityResult);
        _mapperMock.Setup(m => m.Map<LoginResponseDto>(user))
                   .Returns(new LoginResponseDto());

        var result = await _authService.RefreshTokenAsync(refreshToken, CancellationToken.None);

        result.JwtToken.Should().Be(newToken);
        _userRepositoryMock.Verify(r => r.GetByRefreshTokenAsync(refreshToken, It.IsAny<CancellationToken>()), Times.Once);
        _jwtTokenProviderMock.Verify(p => p.GenerateToken(user, It.IsAny<IEnumerable<string>>()), Times.Once);
        _refreshTokenProviderMock.Verify(r=>r.GenerateRefreshToken(), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.Is<User>(u => u.RefreshToken == newRefreshToken)), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Never());
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowLoginException_WhenUserNotFound()
    {
        _userRepositoryMock.Setup(r => r.GetByRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(null as User);

        Func<Task> act = async () => await _authService.RefreshTokenAsync(It.IsAny<string>(), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowLoginException_WhenRefreshTokenExpired()
    {
        var refreshToken = "expired_refresh_token";
        var user = new User
        {
            RefreshToken = refreshToken,
            RefreshTokenExpiredAt = DateTime.Now.AddDays(-1)
        };

        _userRepositoryMock.Setup(r => r.GetByRefreshTokenAsync(refreshToken, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

        Func<Task> act = async () => await _authService.RefreshTokenAsync(refreshToken, CancellationToken.None);

        await act.Should().ThrowAsync<LoginException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }
}