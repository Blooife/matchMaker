using Authentication.BusinessLogic.DTOs.Response;

namespace Authentication.BusinessLogic.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(CancellationToken cancellationToken);
}