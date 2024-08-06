using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;

namespace Match.Domain.Interfaces;

public interface IProfileRepository : IGenericRepository<Profile, string>
{
    Task<(List<string> Ids, int TotalCount)> GetPagedRecsAsync(List<string> excludedProfileIds, Profile userProfile, int pageNumber,
        int pageSize, CancellationToken cancellationToken);
}