using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;
using Shared.Models;

namespace Authentication.BusinessLogic.Services.Implementations;

public class RoleService(IRoleRepository _roleRepository, IUserRepository _userRepository, IMapper _mapper) : IRoleService
{
    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var roles =  await _roleRepository.GetAllRolesAsync(cancellationToken); 
        
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
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