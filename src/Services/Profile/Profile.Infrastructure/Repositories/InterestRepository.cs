using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class InterestRepository : GenericRepository<Interest, int>, IInterestRepository
{
    private readonly ProfileDbContext _dbContext;
    public InterestRepository(ProfileDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddInterestToProfile(UserProfile profile, Interest interest)
    {
        profile.Interests.Add(interest);
    }
    
    public async Task RemoveInterestFromProfile(UserProfile profile, Interest interest)
    {
        profile.Interests.Remove(interest);
    }

    public async Task<List<Interest>> GetUsersInterests(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile!.Interests;
    }
    
    public async Task<UserProfile?> GetUserWithInterests(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}