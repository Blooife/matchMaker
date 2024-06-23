using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;

namespace Match.Domain.Repositories;

public interface IProfileRepository : IGenericRepository<Profile, string>
{
    Task<IEnumerable<Profile>> GetRecommendationsAsync(List<string> excludedProfileIds, Profile userProfile,
        bool locationAccessGranted, CancellationToken cancellationToken);
}