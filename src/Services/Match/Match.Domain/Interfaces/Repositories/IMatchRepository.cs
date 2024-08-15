using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;

namespace Match.Domain.Interfaces.Repositories;

public interface IMatchRepository : IGenericRepository<MatchEntity, string>
{
    Task<bool> AreProfilesMatchedAsync(string profileId1, string profileId2, CancellationToken cancellationToken);
    Task<(List<MatchEntity>, int)> GetPagedAsync(string profileId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}