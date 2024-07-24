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
        return await _dbContext.Users.Where(u => u.DeletedAt == null).FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
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
        return await _userManager.RemoveFromRoleAsync(user, roleName);
    }

    public async Task<User?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.Where(u => u.DeletedAt == null).FirstOrDefaultAsync(u => u.RefreshToken == token, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.Where(u => u.DeletedAt == null).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
    
    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }
    
    public async Task<IdentityResult> DeleteUserByIdAsync(User user)
    {
        user.DeletedAt = DateTime.UtcNow;
        return await _userManager.UpdateAsync(user);
    }

    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await _userManager.Users.Where(u => u.DeletedAt == null).AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<(List<User> Users, int TotalCount)> GetPagedUsersAsync(int pageNumber, int pageSize)
    {
        var query = _dbContext.Set<User>().Where(u => u.DeletedAt == null);
        var totalCount = query.Count();

        var users = await query
            .OrderBy(e => e.Email)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }
}