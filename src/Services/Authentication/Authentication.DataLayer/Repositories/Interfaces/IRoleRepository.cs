using Authentication.DataLayer.Models;

namespace Authentication.DataLayer.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken);
}