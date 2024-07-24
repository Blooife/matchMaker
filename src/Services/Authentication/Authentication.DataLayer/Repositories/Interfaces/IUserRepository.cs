using Authentication.DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Authentication.DataLayer.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IdentityResult> RegisterAsync(User user, string password);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<IList<string>> GetRolesAsync(User user);
    Task<IdentityResult> AddToRoleAsync(User user, string roleName);
    Task<IdentityResult> RemoveFromRoleAsync(User user, string roleName);
    Task<User?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteUserByIdAsync(User user);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<(List<User> Users, int TotalCount)> GetPagedUsersAsync(int pageNumber, int pageSize);
}