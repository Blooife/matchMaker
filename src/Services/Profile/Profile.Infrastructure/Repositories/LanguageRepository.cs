using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class LanguageRepository(ProfileDbContext _dbContext) : ILanguageRepository
{
    public async Task<IEnumerable<Language>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Languages.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Language?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Languages.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
    
    public async Task<Language?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Languages.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }
    
    public async Task AddLanguageToProfile(UserProfile profile, Language language, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages).FirstOrDefaultAsync(p => p.Id == profile.Id, cancellationToken);
        userProfile?.Languages.Add(language);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveLanguageFromProfile(string profileId, int  languageId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages).FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
    
        if (userProfile == null)
        {
            //
        }
    
        var languageToRemove = userProfile.Languages.FirstOrDefault(i => i.Id == languageId);

        if (languageToRemove == null)
        {
            //
        }

        userProfile.Languages.Remove(languageToRemove);

        _dbContext.Update(userProfile);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<List<Language>> GetUsersLanguages(UserProfile profile, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Languages).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profile.Id, cancellationToken);

        return userProfile!.Languages;
    }
}