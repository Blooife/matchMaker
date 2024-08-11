using Profile.Domain.Models;
using Profile.Domain.Interfaces.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations.Repositories;

public class EducationRepository(ProfileDbContext _dbContext)
    : GenericRepository<Education, int>(_dbContext), IEducationRepository
{
    public async Task AddEducationToProfileAsync(UserProfile profile, ProfileEducation userEducation)
    {
        _dbContext.Profiles.Attach(profile);
        profile.ProfileEducations.Add(userEducation);
    }
    
    public async Task RemoveEducationFromProfileAsync(UserProfile profile, ProfileEducation userEducation)
    {
        _dbContext.Profiles.Attach(profile);
        profile.ProfileEducations.Remove(userEducation);
    }
    
    public async Task UpdateProfilesEducationAsync(ProfileEducation userEducation, string description)
    {
        _dbContext.Attach(userEducation);
        userEducation.Description = description;
    }
}