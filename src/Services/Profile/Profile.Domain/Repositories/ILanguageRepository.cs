using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface ILanguageRepository : IGenericRepository<Language, int>
{
    Task<Language?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task AddLanguageToProfile(UserProfile profile, Language language);
    Task RemoveLanguageFromProfile(UserProfile profile, Language language);
    Task<List<Language>> GetUsersLanguages(UserProfile profile, CancellationToken cancellationToken);
    Task<UserProfile?> GetUserWithLanguages(string profileId, CancellationToken cancellationToken);
}
