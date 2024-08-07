using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Producers;
using Authentication.BusinessLogic.Providers.Interfaces;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Messages.Authentication;
using Shared.Models;

namespace Authentication.BusinessLogic.Services.Implementations;

public class AuthService(IUserRepository _userRepository, IMapper _mapper, ILogger<AuthService> _logger,
    IJwtTokenProvider _jwtTokenProvider, IRefreshTokenProvider _refreshTokenProvider, IValidator<UserRequestDto> _validator, ProducerService _producerService) : IAuthService
{
    public async Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto)
    {
        await _validator.ValidateAndThrowAsync(registrationRequestDto);
        var user = _mapper.Map<User>(registrationRequestDto);
        
        var result = await _userRepository.RegisterAsync(user, registrationRequestDto.Password);
        
        if (!result.Succeeded)
        {
            _logger.LogError("User registration failed with errors: {errors}", result.Errors.Select(e => e.Description).ToArray());
            throw new RegisterException(result.Errors.First().Description);
        }
        
        await _userRepository.AddToRoleAsync(user, Roles.User);
        
        var message = _mapper.Map<UserCreatedMessage>(user);
        await _producerService.ProduceAsync(message);
        
        return new  GeneralResponseDto() { Message = "User registered successfully"};
    }

    public async Task<LoginResponseDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(loginRequestDto, cancellationToken);
        var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email, cancellationToken);
            
        if (user is null)
        {
            _logger.LogError("Login failed: user with email = {email} was not found", loginRequestDto.Email);
            throw new LoginException(ExceptionMessages.LoginFailed);
        }

        var isValid = await _userRepository.CheckPasswordAsync(user, loginRequestDto.Password);

        if (isValid == false)
        {
            _logger.LogError("Login failed: Incorrect password for user with email = {email}", loginRequestDto.Email);
            throw new LoginException(ExceptionMessages.LoginFailed);
        }
        
        var roles = await _userRepository.GetRolesAsync(user);
        var token = _jwtTokenProvider.GenerateToken(user, roles);
            
        var refreshToken = _refreshTokenProvider.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiredAt = DateTime.Now.AddDays(7).ToUniversalTime();

        await _userRepository.UpdateUserAsync(user);
            
        var loginResponseDto = _mapper.Map<LoginResponseDto>(user);
        loginResponseDto.JwtToken = token;

        return loginResponseDto;
    }
    
    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);
            
        if (user is null)
        {
            _logger.LogError("Refresh failed: user with refresh token = {token} was not found", refreshToken);
            throw new LoginException(ExceptionMessages.LoginFailed);
        }
            
        if(user.RefreshTokenExpiredAt < DateTime.Now)
        {
            _logger.LogError("Refresh failed: refresh token expired at {at}", user.RefreshTokenExpiredAt);
            throw new LoginException("Refresh token expired");
        }

        var roles = await _userRepository.GetRolesAsync(user);
        var token = _jwtTokenProvider.GenerateToken(user, roles);
            
        var refreshTokenGenerated = _refreshTokenProvider.GenerateRefreshToken();
        user.RefreshToken = refreshTokenGenerated;
        user.RefreshTokenExpiredAt = DateTime.Now.AddDays(7).ToUniversalTime();

        await _userRepository.UpdateUserAsync(user);
            
        var loginResponseDto = _mapper.Map<LoginResponseDto>(user);
        loginResponseDto.JwtToken = token;

        return loginResponseDto;
    }
}