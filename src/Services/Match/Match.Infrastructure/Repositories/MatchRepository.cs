using System.Linq.Expressions;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;
using Match.Domain.Models;

namespace Match.Infrastructure.Repositories;

public class MatchRepository(IMongoCollection<CoupleMatch> _collection) : GenericRepository<CoupleMatch, int>(_collection), IMatchRepository
{
    public async Task<IEnumerable<CoupleMatch>> GetMatchesByProfileIdAsync(string profileId, CancellationToken cancellationToken)
    {
        var filter = Builders<CoupleMatch>.Filter.Or(
            Builders<CoupleMatch>.Filter.Eq(c => c.ProfileId1, profileId),
            Builders<CoupleMatch>.Filter.Eq(c => c.ProfileId2, profileId)
        );

        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<bool> AreProfilesMatched(string profileId1, string profileId2, CancellationToken cancellationToken)
    {
        var filter = Builders<CoupleMatch>.Filter.Or(
            Builders<CoupleMatch>.Filter.And(
                Builders<CoupleMatch>.Filter.Eq(match => match.ProfileId1, profileId1),
                Builders<CoupleMatch>.Filter.Eq(match => match.ProfileId2, profileId2)
            ),
            Builders<CoupleMatch>.Filter.And(
                Builders<CoupleMatch>.Filter.Eq(match => match.ProfileId1, profileId2),
                Builders<CoupleMatch>.Filter.Eq(match => match.ProfileId2, profileId1)
            )
        );

        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }
}