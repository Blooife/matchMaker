using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;

namespace Match.Domain.Repositories;

public interface IProfileRepository : IGenericRepository<Profile, string>
{
    Task<(List<string> Ids, int TotalCount)> GetPagedRecsAsync(List<string> excludedProfileIds, Profile userProfile, int pageNumber,
        int pageSize, CancellationToken cancellationToken);
}