using Authentication.DataLayer.Contexts;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataLayer.Repositories.Implementations;

public class UserRepository(AuthContext _dbContext, UserManager<User> _userManager) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IdentityResult> RegisterAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
    {
        return await _userManager.AddToRoleAsync(user, roleName);
    }
    
    public async Task<IdentityResult> RemoveFromRoleAsync(User user, string roleName)
    {
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count == 1)
        {
            return IdentityResult.Failed(new IdentityError(){Description = "User can't have less than 1 role"});
        }
        return await _userManager.RemoveFromRoleAsync(user, roleName);
    }

    public async Task<User?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == token, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
    
    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }
    
    public async Task<IdentityResult> DeleteUserByIdAsync(User user)
    {
        return await _userManager.DeleteAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await _userManager.Users.AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<User>> GetPaginatedUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        IQueryable<User> query = _dbContext.Set<User>();
        return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .ToListAsync(cancellationToken);
    }
    
}