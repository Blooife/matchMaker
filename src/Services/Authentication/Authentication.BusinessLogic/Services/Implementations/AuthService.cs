using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Providers.Interfaces;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;
using Shared.Constants;

namespace Authentication.BusinessLogic.Services.Implementations;

public class AuthService(IUserRepository _userRepository, IRoleRepository _roleRepository, IMapper _mapper,
    IJwtTokenProvider _jwtTokenProvider, IRefreshTokenProvider _refreshTokenProvider) : IAuthService
{
    public async Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto)
    {
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
    
    public async Task<LoginResponseDto> RefreshToken(string refreshToken, CancellationToken cancellationToken)
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

    public async Task<GeneralResponseDto> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException(userId);
        }

        var isRoleExist = await _roleRepository.RoleExistsAsync(roleName);
        
        if (!isRoleExist)
        {
            throw new AssignRoleException(ExceptionMessages.RoleNotExists);
        }
        
        var result = await _userRepository.AddToRoleAsync(user, roleName);
        
        if (!result.Succeeded)
        {
            throw new AssignRoleException();
        }
        
        return new GeneralResponseDto { Message = "Role assigned successfully" };
    }
    
    public async Task<GeneralResponseDto> RemoveUserFromRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException(userId);
        }

        var roles = await _userRepository.GetRolesAsync(user);
        
        if (roles.Count == 1)
        {
            throw new RemoveRoleException("User can't have less than 1 role");
        }
        
        var result = await _userRepository.RemoveFromRoleAsync(user, roleName);
        
        if (!result.Succeeded)
        {
            throw new RemoveRoleException(result.Errors.First().Description);
        }
        
        return new GeneralResponseDto { Message = "Role removed successfully" };
    }
}