using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface ILanguageRepository : IGenericRepository<Language, int>
{
    Task AddLanguageToProfile(UserProfile profile, Language language);
    Task RemoveLanguageFromProfile(UserProfile profile, Language language);
    Task<List<Language>> GetProfilesLanguages(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithLanguages(string profileId, CancellationToken cancellationToken);
}