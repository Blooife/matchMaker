using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;

namespace Match.Domain.Repositories;

public interface IMatchRepository : IGenericRepository<CoupleMatch, int>
{
    Task<IEnumerable<CoupleMatch>> GetMatchesByProfileIdAsync(string profileId, CancellationToken cancellationToken);
    Task<bool> AreProfilesMatched(string profileId1, string profileId2, CancellationToken cancellationToken);
}