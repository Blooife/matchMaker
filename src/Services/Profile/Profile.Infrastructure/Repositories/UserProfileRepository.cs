using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class UserProfileRepository(ProfileDbContext _dbContext, ICacheService _cacheService)
    : GenericRepository<UserProfile, string>(_dbContext, _cacheService), IUserProfileRepository
{
    private readonly string _cacheKeyPrefix = typeof(UserProfile).Name;
    
    public async Task<UserProfile> UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        _dbContext.Update(profile);
        
        var cacheKey = $"{_cacheKeyPrefix}_{profile.Id}";
        await _cacheService.SetAsync(cacheKey, profile, cancellationToken);
        
        return profile;
    }

    public async Task DeleteProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        profile.DeletedAt = DateTime.UtcNow;
        _dbContext.Update(profile);
        
        var cacheKey = $"{_cacheKeyPrefix}_{profile.Id}";
        await _cacheService.RemoveAsync(cacheKey, cancellationToken);
    }

    public async Task<UserProfile> CreateProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        await _dbContext.Profiles.AddAsync(profile, cancellationToken);
        
        return profile;
    }   
}