using Authentication.BusinessLogic.DTOs;
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

public class UserService(IUserRepository _userRepository, IRoleRepository _roleRepository, IMapper _mapper,
    IJwtTokenProvider _jwtTokenProvider, IRefreshTokenProvider _refreshTokenProvider) : IUserService
{
    public async Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto)
    {
        var user = _mapper.Map<User>(registrationRequestDto);
        
        var result = await _userRepository.RegisterAsync(user, registrationRequestDto.Password);
        if (!result.Succeeded)
        {
            throw new RegisterException(result.Errors.FirstOrDefault().Description);
        }
        await _userRepository.AddToRoleAsync(user, Roles.User);
        
        return new  GeneralResponseDto() { Message = "User registered successfully"};
    }

    public async Task<UserDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email, cancellationToken);
            
        if (user == null)
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
            
        var userDto = _mapper.Map<UserDto>(user);
        userDto.JwtToken = token;

        return userDto;
    }

    public async Task<GeneralResponseDto> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user != null)
        {
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
        throw new NotFoundException(userId);
    }
    
    public async Task<GeneralResponseDto> RemoveUserFromRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user != null)
        {
            var roles = await _userRepository.GetRolesAsync(user);
            if (roles.Count == 1)
            {
                throw new RemoveRoleException();
            }
            var result = await _userRepository.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new RemoveRoleException();
            }
            return new GeneralResponseDto { Message = "Role removed successfully" };
        }
        throw new NotFoundException(userId);
    }
    
    public async Task<GeneralResponseDto> DeleteUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(userId);
        }

        var result = await _userRepository.DeleteUserByIdAsync(user);
        if (!result.Succeeded)
        {
            throw new DeleteUserException(ExceptionMessages.DeleteUserFailed);
        }
        
        return new GeneralResponseDto() { Message = "User deleted successfully" };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        return _mapper.Map<UserDto>(user);
    }
    
    public async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetUsersRoles(string userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(userId);
        }

        var roles = _userRepository.GetRolesAsync(user);
        
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
    }
    
    public async Task<UserDto> RefreshToken(string refrToken, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refrToken, cancellationToken);
            
        if (user == null)
        {
            throw new LoginException(ExceptionMessages.LoginFailed);
        }
            
        if(user.RefreshTokenExpiredAt < DateTime.Now)
        {
            throw new LoginException("Refresh token expired");
        }

        var roles = await _userRepository.GetRolesAsync(user);
        var token = _jwtTokenProvider.GenerateToken(user, roles);
            
        var refreshToken = _refreshTokenProvider.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiredAt = DateTime.Now.AddDays(7).ToUniversalTime();

        await _userRepository.UpdateUserAsync(user);
            
        var userDto = _mapper.Map<UserDto>(user);
        userDto.JwtToken = token;

        return userDto;
    }
}