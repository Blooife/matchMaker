using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface ILanguageRepository : IGenericRepository<Language, int>
{
    Task AddLanguageToProfileAsync(UserProfile profile, Language language);
    Task RemoveLanguageFromProfileAsync(UserProfile profile, Language language, CancellationToken cancellationToken);
    Task<List<Language>> GetProfilesLanguagesAsync(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithLanguagesAsync(string profileId, CancellationToken cancellationToken);
}