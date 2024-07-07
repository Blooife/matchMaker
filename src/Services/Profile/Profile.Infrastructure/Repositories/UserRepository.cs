using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class UserRepository(ProfileDbContext _dbContext,  ICacheService _cacheService)
    : GenericRepository<User, string>(_dbContext, _cacheService), IUserRepository
{
    private readonly string _cacheKeyPrefix = typeof(User).Name;
    
    public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
    {
        user.DeletedAt = DateTime.UtcNow;
        _dbContext.Update(user);
        
        var cacheKey = $"{_cacheKeyPrefix}_{user.Id}";
        await _cacheService.RemoveAsync(cacheKey, cancellationToken);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        
        return user;
    }
}