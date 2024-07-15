using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations;

public class UserRepository(ProfileDbContext _dbContext) : GenericRepository<User, string>(_dbContext), IUserRepository
{
    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Update(user);
        
        return user;
    }

    public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Remove(user);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        
        return user;
    }
}