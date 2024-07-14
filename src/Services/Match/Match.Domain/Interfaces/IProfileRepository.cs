using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;
using Shared.Models;

namespace Match.Domain.Interfaces;

public interface IProfileRepository : IGenericRepository<Profile, string>
{
    Task<IEnumerable<Profile>> GetRecommendationsAsync(List<string> excludedProfileIds, Profile userProfile,
        bool locationAccessGranted, CancellationToken cancellationToken);
    Task<PagedList<Profile>> GetPagedRecsAsync(List<string> excludedProfileIds, Profile userProfile, int pageNumber,
        int pageSize, CancellationToken cancellationToken);
}