using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface IUserRepository : IGenericRepository<User, string>
{
    Task DeleteUserAsync(User user, CancellationToken cancellationToken);
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
}