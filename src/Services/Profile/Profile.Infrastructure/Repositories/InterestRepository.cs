using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class InterestRepository(ProfileDbContext _dbContext)
    : GenericRepository<Interest, int>(_dbContext), IInterestRepository
{
    public async Task AddInterestToProfileAsync(UserProfile profile, Interest interest)
    {
        profile.Interests.Add(interest);
    }
    
    public async Task RemoveInterestFromProfileAsync(UserProfile profile, Interest interest, CancellationToken cancellationToken)
    {
        profile.Interests.Remove(interest);
    }

    public async Task<List<Interest>> GetProfilesInterestsAsync(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile!.Interests;
    }
    
    public async Task<UserProfile?> GetProfileWithInterestsAsync(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}