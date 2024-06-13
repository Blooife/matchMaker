using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class InterestRepository(ProfileDbContext _dbContext) : IInterestRepository
{
    public async Task<IEnumerable<Interest>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Interests.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Interest?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Interests.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
    
    public async Task<Interest?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Interests.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }
    
    public async Task AddInterestToProfile(UserProfile profile, Interest interest, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests).FirstOrDefaultAsync(p => p.Id == profile.Id, cancellationToken);
        userProfile?.Interests.Add(interest);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveInterestFromProfile(string profileId, int  interestId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests).FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
    
        if (userProfile == null)
        {
            //
        }
    
        var interestToRemove = userProfile.Interests.FirstOrDefault(i => i.Id == interestId);

        if (interestToRemove == null)
        {
            //
        }

        userProfile.Interests.Remove(interestToRemove);

        _dbContext.Update(userProfile);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Interest>> GetUsersInterests(UserProfile profile, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Interests).AsNoTracking()
            .FirstOrDefaultAsync(p => p == profile, cancellationToken);

        return userProfile!.Interests;
    }
}