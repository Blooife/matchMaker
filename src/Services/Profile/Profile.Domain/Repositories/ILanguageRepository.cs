using Profile.Domain.Models;

namespace Profile.Domain.Repositories;

public interface ILanguageRepository
{
    Task<IEnumerable<Language>> GetAllAsync(CancellationToken cancellationToken);
    Task<Language?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Language?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task AddLanguageToProfile(UserProfile profile, Language language, CancellationToken cancellationToken);
    Task RemoveLanguageFromProfile(string profileId, int languageId, CancellationToken cancellationToken);
    Task<List<Language>> GetUsersLanguages(UserProfile profile, CancellationToken cancellationToken);
}
