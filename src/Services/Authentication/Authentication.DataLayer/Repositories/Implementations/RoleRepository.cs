using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataLayer.Repositories.Implementations;

public class RoleRepository(RoleManager<Role> _roleManager) : IRoleRepository
{
    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<IdentityResult> CreateRoleAsync(Role role)
    {
        return await _roleManager.CreateAsync(role);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        return await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
    }
}