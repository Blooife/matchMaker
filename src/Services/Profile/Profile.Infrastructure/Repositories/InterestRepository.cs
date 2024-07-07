using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class InterestRepository(ProfileDbContext _dbContext, ICacheService _cacheService)
    : GenericRepository<Interest, int>(_dbContext, _cacheService), IInterestRepository
{
    public async Task AddInterestToProfile(UserProfile profile, Interest interest)
    {
        profile.Interests.Add(interest);
    }
    
    public async Task RemoveInterestFromProfile(UserProfile profile, Interest interest, CancellationToken cancellationToken)
    {
        profile.Interests.Remove(interest);
    }

    public async Task<List<Interest>> GetProfilesInterests(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile!.Interests;
    }
    
    public async Task<UserProfile?> GetProfileWithInterests(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}