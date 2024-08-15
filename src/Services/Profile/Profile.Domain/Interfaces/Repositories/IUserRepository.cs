using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User, string>
{
    Task DeleteUserAsync(User user);
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
}