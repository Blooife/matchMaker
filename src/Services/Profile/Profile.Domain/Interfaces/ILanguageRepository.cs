using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface ILanguageRepository : IGenericRepository<Language, int>
{
    Task AddLanguageToProfileAsync(UserProfile profile, Language language);
    Task RemoveLanguageFromProfileAsync(UserProfile profile, Language language, CancellationToken cancellationToken);
}