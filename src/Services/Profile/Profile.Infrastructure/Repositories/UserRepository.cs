using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class UserRepository(ProfileDbContext _dbContext) : GenericRepository<User, string>(_dbContext), IUserRepository
{
    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Update(user);
        
        return user;
    }

    public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
    {
        user.DeletedAt = DateTime.UtcNow;
        _dbContext.Update(user);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        
        return user;
    }
}