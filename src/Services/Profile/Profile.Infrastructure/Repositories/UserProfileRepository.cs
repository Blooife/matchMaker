using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class UserProfileRepository(ProfileDbContext _dbContext) : IUserProfileRepository
{
    public async Task<IEnumerable<UserProfile>> GetAllProfilesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Profiles.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<UserProfile> UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        _dbContext.Update(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return profile;
    }

    public async Task DeleteProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        _dbContext.Profiles.Remove(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserProfile?> GetProfileByIdAsync(string profileId, CancellationToken cancellationToken)
    {
        return await _dbContext.Profiles.AsNoTracking().FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
    }

    public async Task<UserProfile> CreateProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        await _dbContext.Profiles.AddAsync(profile, cancellationToken);
        
        var pref = new Preference();
        pref.ProfileId = profile.Id;
        
        await _dbContext.Preferences.AddAsync(pref, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return profile;
    }
}