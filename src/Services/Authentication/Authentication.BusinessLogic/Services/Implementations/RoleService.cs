using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Authentication.BusinessLogic.Services.Implementations;

public class RoleService(IRoleRepository _roleRepository, IUserRepository _userRepository, IMapper _mapper, ILogger<RoleService> _logger) : IRoleService
{
    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var roles =  await _roleRepository.GetAllRolesAsync(cancellationToken); 
        
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
    }
    
    public async Task<GeneralResponseDto> AssignRoleAsync(string email, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError($"User with email = {email} was not found");
            throw new NotFoundException(email);
        }

        var isRoleExist = await _roleRepository.RoleExistsAsync(roleName);
        
        if (!isRoleExist)
        {
            _logger.LogError($"{roleName} role does not exist");
            throw new AssignRoleException(ExceptionMessages.RoleNotExists);
        }
        
        var result = await _userRepository.AddToRoleAsync(user, roleName);
        
        if (!result.Succeeded)
        {
            _logger.LogError(result.Errors.First().Description);
            throw new AssignRoleException(result.Errors.First().Description);
        }
        
        return new GeneralResponseDto { Message = "Role assigned successfully" };
    }
    
    public async Task<GeneralResponseDto> RemoveUserFromRoleAsync(string email, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError($"User with email = {email} was not found");
            throw new NotFoundException(email);
        }

        var roles = await _userRepository.GetRolesAsync(user);
        
        if (roles.Count == 1)
        {
            _logger.LogError($"User can't have less than 1 role");
            throw new RemoveRoleException("User can't have less than 1 role");
        }
        
        var result = await _userRepository.RemoveFromRoleAsync(user, roleName);
        
        if (!result.Succeeded)
        {
            _logger.LogError(result.Errors.First().Description);
            throw new RemoveRoleException(result.Errors.First().Description);
        }
        
        return new GeneralResponseDto { Message = "Role removed successfully" };
    }
}