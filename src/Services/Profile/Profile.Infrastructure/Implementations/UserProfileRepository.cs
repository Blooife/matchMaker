using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Domain.Specifications;
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
    
    public async Task<UserProfile?> GetAllProfileInfoAsync(Expression<Func<UserProfile, bool>> expression, CancellationToken cancellationToken)
    {
        return await _dbContext.Profiles
            .Include(p => p.Goal)
            .Include(p => p.City)
            .ThenInclude(c => c.Country)
            .Include(p=>p.ProfileEducations)
            .ThenInclude(pe=>pe.Education)
            .Include(p => p.Languages)
            .Include(p => p.Interests)
            .Include(p => p.Images.OrderByDescending(i=>i.IsMainImage).ThenByDescending(i => i.UploadTimestamp))
            .Where(expression).WhereNotDeleted().AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<UserProfile>> GetAllProfileInfoByIdsAsync(IEnumerable<string> profileIds)
    {
        return await _dbContext.Profiles
            .Include(p => p.Goal)
            .Include(p => p.City)
            .ThenInclude(c => c.Country)
            .Include(p=>p.ProfileEducations)
            .ThenInclude(pe=>pe.Education)
            .Include(p => p.Languages)
            .Include(p => p.Interests)
            .Include(p => p.Images.OrderByDescending(i=>i.IsMainImage).ThenByDescending(i => i.UploadTimestamp))
            .Where(profile=>profileIds.Contains(profile.Id)).WhereNotDeleted().AsNoTracking().ToListAsync();
    }
}