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

public class UserService(IUserRepository _userRepository, IMapper _mapper) : IUserService
{
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
    
    public async Task<GeneralResponseDto> UpdateUser(UserRequestDto userDto)
    {
        var user = _mapper.Map<User>(userDto);

        var result = await _userRepository.UpdateUserAsync(user);
        
        if (!result.Succeeded)
        {
            throw new UpdateUserException(ExceptionMessages.UpdateUserFailed);
        }
        
        return new GeneralResponseDto() { Message = "User updated successfully" };
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }
    
    public async Task<IEnumerable<UserResponseDto>> GetPaginatedUsersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetPaginatedUsersAsync(pageSize, pageNumber, cancellationToken);
        
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        return _mapper.Map<UserResponseDto>(user);
    }
    
    public async Task<UserResponseDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        return _mapper.Map<UserResponseDto>(user);
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
}