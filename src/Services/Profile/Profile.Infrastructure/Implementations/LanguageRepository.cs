using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations;

public class LanguageRepository : GenericRepository<Language, int>, ILanguageRepository
{
    private readonly ProfileDbContext _dbContext;
    public LanguageRepository(ProfileDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddLanguageToProfile(UserProfile profile, Language language)
    {
        profile.Languages.Add(language);
    }
    
    public async Task RemoveLanguageFromProfile(UserProfile profile, Language language)
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