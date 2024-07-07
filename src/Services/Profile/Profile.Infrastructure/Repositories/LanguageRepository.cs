using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class LanguageRepository(ProfileDbContext _dbContext, ICacheService _cacheService)
    : GenericRepository<Language, int>(_dbContext, _cacheService), ILanguageRepository
{
    public async Task AddLanguageToProfile(UserProfile profile, Language language)
    {
        profile.Languages.Add(language);
    }
    
    public async Task RemoveLanguageFromProfile(UserProfile profile, Language language, CancellationToken cancellationToken)
    {
        profile.Languages.Remove(language);
    }
    
    public async Task<List<Language>> GetProfilesLanguages(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile!.Languages;
    }
    
    public async Task<UserProfile?> GetProfileWithLanguages(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}