using Match.Domain.Models;
using Match.Domain.Interfaces;
using Match.Infrastructure.Implementations.BaseRepositories;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using Shared.Constants;
using Shared.Models;

namespace Match.Infrastructure.Implementations;

public class ProfileRepository(IMongoCollection<Profile> _collection) : GenericRepository<Profile, string>(_collection), IProfileRepository
{
    public async Task<IEnumerable<Profile>> GetRecommendationsAsync(List<string> excludedProfileIds, Profile userProfile, bool locationAccessGranted, CancellationToken cancellationToken)
    {
        var filter = GetFilterForRecommendations(excludedProfileIds, userProfile);
        
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }
    
    public async Task<PagedList<Profile>> GetPagedRecsAsync(List<string> excludedProfileIds, Profile userProfile, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var filter = GetFilterForRecommendations(excludedProfileIds, userProfile);

        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<Profile, Profile>()
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize,
        };

        var items = await _collection.Find(filter)
            .Skip(findOptions.Skip)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        return new PagedList<Profile>(items, (int)count, pageNumber, pageSize);
    }

    private FilterDefinition<Profile> GetFilterForRecommendations(List<string> excludedProfileIds, Profile userProfile)
    {
        var filters = new List<FilterDefinition<Profile>>
        {
            Builders<Profile>.Filter.Ne(p => p.Id, userProfile.Id),
            Builders<Profile>.Filter.Nin(p => p.Id, excludedProfileIds), 
        };

        if (userProfile.PreferredGender != Gender.Undefined)
        {
            filters.Add(Builders<Profile>.Filter.Eq(p => p.Gender, userProfile.PreferredGender));
        }
        else
        {
            filters.Add(Builders<Profile>.Filter.Or(
                Builders<Profile>.Filter.Eq(p => p.Gender, Gender.Male),
                Builders<Profile>.Filter.Eq(p => p.Gender, Gender.Female)
            ));
        }

        if (userProfile.AgeFrom != 0 && userProfile.AgeTo != 0)
        {
            filters.Add(Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Gte(p => p.BirthDate, DateTime.Now.AddYears(-userProfile.AgeTo)),
                Builders<Profile>.Filter.Lte(p => p.BirthDate, DateTime.Now.AddYears(-userProfile.AgeFrom)),
                Builders<Profile>.Filter.Gte(p => p.AgeFrom, userProfile.AgeFrom),
                Builders<Profile>.Filter.Lte(p => p.AgeTo, userProfile.AgeTo)
            ));
        }

        if (userProfile is { Location: not null, MaxDistance: > 0 })
        {
            var maxDistanceInMeters = userProfile.MaxDistance * 1000;
            var coordinates = userProfile.Location.Coordinates;
            var point = new GeoJsonPoint<GeoJson2DCoordinates>(coordinates);
            filters.Add(Builders<Profile>.Filter.NearSphere(p => p.Location, point, maxDistanceInMeters));
        }
        else
        {
            filters.Add(Builders<Profile>.Filter.Eq(p => p.Country, userProfile.Country));
        }
        
        return Builders<Profile>.Filter.And(filters);
    }
}