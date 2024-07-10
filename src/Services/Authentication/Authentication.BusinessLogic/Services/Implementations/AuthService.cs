using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Providers.Interfaces;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;
using FluentValidation;
using Shared.Constants;

namespace Authentication.BusinessLogic.Services.Implementations;

public class AuthService(IUserRepository _userRepository, IMapper _mapper,
    IJwtTokenProvider _jwtTokenProvider, IRefreshTokenProvider _refreshTokenProvider, IValidator<UserRequestDto> _validator) : IAuthService
{
    public async Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto)
    {
        await _validator.ValidateAndThrowAsync(registrationRequestDto);
        var user = _mapper.Map<User>(registrationRequestDto);
        
        var result = await _userRepository.RegisterAsync(user, registrationRequestDto.Password);
        
        if (!result.Succeeded)
        {
            throw new RegisterException(result.Errors.First().Description);
        }
        await _userRepository.AddToRoleAsync(user, Roles.User);
        
        return new  GeneralResponseDto() { Message = "User registered successfully"};
    }

    public async Task<LoginResponseDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(loginRequestDto);
        var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email, cancellationToken);
            
        if (user is null)
        {
            throw new LoginException(ExceptionMessages.LoginFailed);
        }

        var isValid = await _userRepository.CheckPasswordAsync(user, loginRequestDto.Password);

        if (isValid == false)
        {
            throw new LoginException(ExceptionMessages.LoginFailed);
        }
        
        var roles = await _userRepository.GetRolesAsync(user);
        var token = _jwtTokenProvider.GenerateToken(user, roles);
            
        var refreshToken = _refreshTokenProvider.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiredAt = DateTime.Now.AddDays(7).ToUniversalTime();

        await _userRepository.UpdateUserAsync(user);
            
        var userDto = _mapper.Map<UserResponseDto>(user);
        var loginResponseDto = new LoginResponseDto()
        {
            userDto = userDto,
            JwtToken = token,
        };

        return loginResponseDto;
    }
    
    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);
            
        if (user is null)
        {
            throw new LoginException(ExceptionMessages.LoginFailed);
        }
            
        if(user.RefreshTokenExpiredAt < DateTime.Now)
        {
            throw new LoginException("Refresh token expired");
        }

        var roles = await _userRepository.GetRolesAsync(user);
        var token = _jwtTokenProvider.GenerateToken(user, roles);
            
        var refreshTokenGenerated = _refreshTokenProvider.GenerateRefreshToken();
        user.RefreshToken = refreshTokenGenerated;
        user.RefreshTokenExpiredAt = DateTime.Now.AddDays(7).ToUniversalTime();

        await _userRepository.UpdateUserAsync(user);
            
        var userDto = _mapper.Map<UserResponseDto>(user);
        var loginResponseDto = new LoginResponseDto()
        {
            userDto = userDto,
            JwtToken = token,
        };

        return loginResponseDto;
    }
}