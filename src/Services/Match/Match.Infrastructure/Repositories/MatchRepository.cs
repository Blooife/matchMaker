using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;
using Match.Domain.Models;

namespace Match.Infrastructure.Repositories;

public class MatchRepository(IMongoCollection<CoupleMatch> _collection) : GenericRepository<CoupleMatch>(_collection), IMatchRepository
{
    public async Task<IEnumerable<CoupleMatch>> GetMatchesByProfileIdAsync(string profileId, CancellationToken cancellationToken)
    {
        var filter = Builders<CoupleMatch>.Filter.Or(
            Builders<CoupleMatch>.Filter.Eq(c => c.ProfileId1, profileId),
            Builders<CoupleMatch>.Filter.Eq(c => c.ProfileId2, profileId)
        );

        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }
}