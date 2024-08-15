using Profile.Domain.Models;
using Profile.Domain.Interfaces.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations.Repositories;

public class UserRepository(ProfileDbContext _dbContext)
    : GenericRepository<User, string>(_dbContext), IUserRepository
{
    public async Task DeleteUserAsync(User user)
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