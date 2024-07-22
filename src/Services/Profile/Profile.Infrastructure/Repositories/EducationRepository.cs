using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class EducationRepository(ProfileDbContext _dbContext)
    : GenericRepository<Education, int>(_dbContext), IEducationRepository
{
    public async Task AddEducationToProfileAsync(UserProfile profile, ProfileEducation userEducation)
    {
        profile.ProfileEducations.Add(userEducation);
        _dbContext.Attach(profile);
    }
    
    public async Task RemoveEducationFromProfileAsync(UserProfile profile, ProfileEducation userEducation)
    {
        profile.ProfileEducations.Remove(userEducation);
        _dbContext.Attach(profile);
    }
    
    public async Task UpdateProfilesEducationAsync(ProfileEducation userEducation, string description)
    {
        userEducation.Description = description;
    }
    
    public async Task<List<ProfileEducation>> GetProfilesEducationAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.ProfileEducations).ThenInclude(ue=>ue.Education).AsNoTracking()
            .FirstOrDefaultAsync(p => p == profile, cancellationToken);

        return userProfile!.ProfileEducations;
    }
    
    public async Task<UserProfile?> GetProfileWithEducationAsync(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.ProfileEducations).ThenInclude(ue=>ue.Education)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}