using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;

namespace Match.Domain.Repositories;

public interface IMatchRepository : IGenericRepository<CoupleMatch>
{
    Task<IEnumerable<CoupleMatch>> GetMatchesByProfileIdAsync(string profileId, CancellationToken cancellationToken);
}