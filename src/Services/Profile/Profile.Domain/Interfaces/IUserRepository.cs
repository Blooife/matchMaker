using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User, string>
{
    Task DeleteUserAsync(User user, CancellationToken cancellationToken);
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
}