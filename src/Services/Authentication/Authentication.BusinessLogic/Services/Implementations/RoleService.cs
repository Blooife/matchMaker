using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;

namespace Authentication.BusinessLogic.Services.Implementations;

public class RoleService(IRoleRepository _roleRepository, IMapper _mapper) : IRoleService
{
    public async Task<GeneralResponseDto> CreateRoleAsync(string roleName)
    {
        var role = _mapper.Map<Role>(roleName);
        var result = await _roleRepository.CreateRoleAsync(role);
        if (!result.Succeeded)
        {
            throw new CreateRoleException(ExceptionMessages.CreateRoleFailed);
        }
        return new GeneralResponseDto() { Message = "Role created successfully" };
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var roles =  await _roleRepository.GetAllRolesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
    }
}