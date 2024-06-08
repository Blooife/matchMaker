using DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<IdentityResult> CreateRoleAsync(Role role);
    Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken);
}