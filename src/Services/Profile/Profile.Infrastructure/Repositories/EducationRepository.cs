using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class EducationRepository(ProfileDbContext _dbContext, ICacheService _cacheService)
    : GenericRepository<Education, int>(_dbContext, _cacheService), IEducationRepository
{
    public async Task AddEducationToProfile(UserProfile profile, ProfileEducation userEducation)
    {
        profile.ProfileEducations.Add(userEducation);
    }
    
    public async Task RemoveEducationFromProfile(UserProfile profile, ProfileEducation userEducation)
    {
        profile.ProfileEducations.Remove(userEducation);
    }
    
    public async Task UpdateProfilesEducation(ProfileEducation userEducation, string description)
    {
        userEducation.Description = description;
    }
    
    public async Task<List<ProfileEducation>> GetProfilesEducation(UserProfile profile, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.ProfileEducations).ThenInclude(ue=>ue.Education).AsNoTracking()
            .FirstOrDefaultAsync(p => p == profile, cancellationToken);

        return userProfile!.ProfileEducations;
    }
    
    public async Task<UserProfile?> GetProfileWithEducation(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.ProfileEducations)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}