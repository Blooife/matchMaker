using System.Linq.Expressions;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;
using Match.Domain.Models;
using Shared.Models;

namespace Match.Infrastructure.Repositories;

public class MatchRepository(IMongoCollection<MatchEntity> _collection) : GenericRepository<MatchEntity, string>(_collection), IMatchRepository
{
    public async Task<IEnumerable<MatchEntity>> GetMatchesByProfileIdAsync(string profileId, CancellationToken cancellationToken)
    {
        var filter = Builders<MatchEntity>.Filter.Or(
            Builders<MatchEntity>.Filter.Eq(match => match.ProfileId1, profileId),
            Builders<MatchEntity>.Filter.Eq(match => match.ProfileId2, profileId)
        );

        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<bool> AreProfilesMatched(string profileId1, string profileId2, CancellationToken cancellationToken)
    {
        var filter = Builders<MatchEntity>.Filter.Or(
            Builders<MatchEntity>.Filter.And(
                Builders<MatchEntity>.Filter.Eq(match => match.ProfileId1, profileId1),
                Builders<MatchEntity>.Filter.Eq(match => match.ProfileId2, profileId2)
            ),
            Builders<MatchEntity>.Filter.And(
                Builders<MatchEntity>.Filter.Eq(match => match.ProfileId1, profileId2),
                Builders<MatchEntity>.Filter.Eq(match => match.ProfileId2, profileId1)
            )
        );

        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }
    
    public async Task<PagedList<MatchEntity>> GetPagedAsync(string profileId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        Expression<Func<MatchEntity, bool>> filter = match => match.ProfileId1 == profileId || match.ProfileId2 == profileId;

        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<MatchEntity, MatchEntity>()
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize,
            Sort = Builders<MatchEntity>.Sort.Descending(match => match.Timestamp)
        };

        var items = await _collection.Find(filter)
            .Sort(findOptions.Sort)
            .Skip(findOptions.Skip)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        return new PagedList<MatchEntity>(items, (int)count, pageNumber, pageSize);
    }
}