using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class EducationRepository(ProfileDbContext _dbContext) : IEducationRepository
{
    public async Task<IEnumerable<Education>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Educations.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Education?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Educations.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
    
    public async Task<Education?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Educations.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }
    
    public async Task AddEducationToProfile(UserProfile profile, Education education, string description, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.UserEducations).FirstOrDefaultAsync(p => p.Id == profile.Id, cancellationToken);
        userProfile?.UserEducations.Add(new UserEducation()
        {
            EducationId = education.Id,
            ProfileId = profile.Id,
            Description = description,
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveEducationFromProfile(string profileId, int  educationId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.UserEducations).FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
    
        if (userProfile == null)
        {
            //
        }
    
        var educationToRemove = userProfile.UserEducations.FirstOrDefault(i => i.EducationId == educationId);

        if (educationToRemove == null)
        {
            //
        }

        userProfile.UserEducations.Remove(educationToRemove);

        _dbContext.Update(userProfile);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<List<UserEducation>> GetUsersEducation(UserProfile profile, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.UserEducations).AsNoTracking()
            .FirstOrDefaultAsync(p => p == profile, cancellationToken);

        return userProfile!.UserEducations;
    }
}