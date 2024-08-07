using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Producers;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;
using Shared.Messages.Authentication;
using Microsoft.Extensions.Logging;
using Shared.Models;

namespace Authentication.BusinessLogic.Services.Implementations;

public class UserService(IUserRepository _userRepository, IMapper _mapper, ILogger<UserService> _logger, ProducerService _producerService) : IUserService

{
    public async Task<GeneralResponseDto> DeleteUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError("Delete user by id failed: User with id = {userId} was not found", userId);
            throw new NotFoundException(userId);
        }

        var result = await _userRepository.DeleteUserByIdAsync(user);
        
        if (!result.Succeeded)
        {
            _logger.LogError("Delete user by id failed with errors: {errors}", result.Errors.Select(e => e.Description).ToArray());
            throw new DeleteUserException(ExceptionMessages.DeleteUserFailed);
        }

        var message = _mapper.Map<UserDeletedMessage>(user);
        await _producerService.ProduceAsync(message);
        
        return new GeneralResponseDto() { Message = "User deleted successfully" };
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersAsync(cancellationToken);
        
        var mappedUsers = _mapper.Map<List<UserResponseDto>>(users);
        
        for (var i = 0; i < mappedUsers.Count; i++)
        {
            mappedUsers[i].Roles = await _userRepository.GetRolesAsync(users[i]);
        }
        
        return mappedUsers;
    }
    
    public async Task<PagedList<UserResponseDto>> GetPaginatedUsersAsync(int pageSize, int pageNumber)
    {
        var (users, totalCount) = await _userRepository.GetPagedUsersAsync(pageNumber, pageSize);

        var userResponseDtos = users.Select(user => new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
        }).ToList();

        for (int i = 0; i < userResponseDtos.Count; i++)
        {
            userResponseDtos[i].Roles = await _userRepository.GetRolesAsync(users[i]);
        }
        
        return new PagedList<UserResponseDto>(userResponseDtos, totalCount, pageNumber, pageSize);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            _logger.LogError("Get user by id failed: User with id = {userId} was not found", userId);
            throw new NotFoundException(userId);
        }
        
        return _mapper.Map<UserResponseDto>(user);
    }
    
    public async Task<UserResponseDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError("Get user by email failed: User with email = {email} was not found", email);
            throw new NotFoundException(email);
        }
        
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetUsersRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError("Get user's roles failed: User with id = {userId} was not found", userId);
            throw new NotFoundException(userId);
        }

        var roles = _userRepository.GetRolesAsync(user);
        
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
    }
}