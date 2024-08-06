using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations;

public class LanguageRepository(ProfileDbContext _dbContext)
    : GenericRepository<Language, int>(_dbContext), ILanguageRepository
{
    public async Task AddLanguageToProfileAsync(UserProfile profile, Language language)
    {
        profile.Languages.Add(language);
    }
    
    public async Task RemoveLanguageFromProfileAsync(UserProfile profile, Language language, CancellationToken cancellationToken)
    {
        profile.Languages.Remove(language);
    }
    
    public async Task<List<Language>> GetProfilesLanguagesAsync(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile!.Languages;
    }
    
    public async Task<UserProfile?> GetProfileWithLanguagesAsync(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}