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
        _dbContext.Profiles.Attach(profile);
        profile.Languages.Add(language);
    }
    
    public async Task RemoveLanguageFromProfileAsync(UserProfile profile, Language language, CancellationToken cancellationToken)
    {
        _dbContext.Profiles.Attach(profile);
        profile.Languages.Remove(language);
    }
}