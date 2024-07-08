using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

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
}