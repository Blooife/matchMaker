using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface ILanguageRepository : IGenericRepository<Language, int>
{
    Task AddLanguageToProfile(UserProfile profile, Language language);
    Task RemoveLanguageFromProfile(UserProfile profile, Language language);
    Task<List<Language>> GetProfilesLanguages(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithLanguages(string profileId, CancellationToken cancellationToken);
}