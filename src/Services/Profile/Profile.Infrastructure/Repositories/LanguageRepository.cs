using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class LanguageRepository : GenericRepository<Language, int>, ILanguageRepository
{
    private readonly ProfileDbContext _dbContext;
    public LanguageRepository(ProfileDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Language?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Languages.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }
    
    public async Task AddLanguageToProfile(UserProfile profile, Language language)
    {
        profile.Languages.Add(language);
    }
    
    public async Task RemoveLanguageFromProfile(UserProfile profile, Language language)
    {
        profile.Languages.Remove(language);
    }
    
    public async Task<List<Language>> GetUsersLanguages(UserProfile profile, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profile.Id, cancellationToken);

        return userProfile!.Languages;
    }
    
    public async Task<UserProfile?> GetUserWithLanguages(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}