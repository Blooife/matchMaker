using Authentication.BusinessLogic.DTOs.Response;

namespace Authentication.BusinessLogic.Services.Interfaces;

public interface IRoleService
{
    Task<GeneralResponseDto> CreateRoleAsync(string roleName);
    Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(CancellationToken cancellationToken);
}