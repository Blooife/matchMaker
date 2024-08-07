using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations;

public class UserProfileRepository(ProfileDbContext _dbContext)
    : GenericRepository<UserProfile, string>(_dbContext), IUserProfileRepository
{
    public async Task<UserProfile> UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        _dbContext.Update(profile);
        
        return profile;
    }

    public async Task DeleteProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        profile.DeletedAt = DateTime.UtcNow;
        _dbContext.Update(profile);
    }

    public async Task<UserProfile> CreateProfileAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        await _dbContext.Profiles.AddAsync(profile, cancellationToken);
        
        return profile;
    }   
    
    public async Task<IEnumerable<UserProfile>> GetAllProfileInfoByIdsAsync(IEnumerable<string> profileIds)
    {
        return await _dbContext.Profiles
            .Include(p => p.Goal)
            .Include(p => p.City)
            .ThenInclude(c => c.Country)
            .Include(p => p.Languages)
            .Include(p => p.Interests)
            .Include(p => p.Images)
            .Where(profile=>profileIds.Contains(profile.Id)).AsNoTracking().ToListAsync();
    }
}