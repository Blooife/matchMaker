using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class EducationRepository : GenericRepository<Education, int>, IEducationRepository
{
    private readonly ProfileDbContext _dbContext;
    public EducationRepository(ProfileDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Education?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Educations.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }
    
    public async Task AddEducationToProfile(UserProfile profile, UserEducation userEducation)
    {
        profile.UserEducations.Add(userEducation);
    }
    
    public async Task RemoveEducationFromProfile(UserProfile profile, UserEducation userEducation)
    {
        profile.UserEducations.Remove(userEducation);
    }
    
    public async Task UpdateUsersEducation(UserEducation userEducation, string description)
    {
        userEducation.Description = description;
    }
    
    public async Task<List<UserEducation>> GetUsersEducation(UserProfile profile, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.UserEducations).AsNoTracking()
            .FirstOrDefaultAsync(p => p == profile, cancellationToken);

        return userProfile!.UserEducations;
    }
    
    public async Task<UserProfile?> GetUserWithEducation(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.UserEducations)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}