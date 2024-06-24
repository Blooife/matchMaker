using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;

namespace Authentication.BusinessLogic.Services.Implementations;

public class RoleService(IRoleRepository _roleRepository, IMapper _mapper) : IRoleService
{
    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var roles =  await _roleRepository.GetAllRolesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
    }
}