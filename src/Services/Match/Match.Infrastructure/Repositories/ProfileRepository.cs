using Match.Domain.Models;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Match.Infrastructure.Repositories;

public class ProfileRepository(IMongoCollection<Profile> _collection) : GenericRepository<Profile, string>(_collection), IProfileRepository
{
    public async Task<IEnumerable<Profile>> GetRecommendationsAsync(List<string> excludedProfileIds, Profile userProfile, bool locationAccessGranted, CancellationToken cancellationToken)
    {
        var filter = GetFilterForRecommendations(excludedProfileIds, userProfile, locationAccessGranted);
        
        return await _collection.Find(filter).Limit(50).ToListAsync(cancellationToken);
    }

    private FilterDefinition<Profile> GetFilterForRecommendations(List<string> excludedProfileIds, Profile userProfile, bool locationAccessGranted)
    {
        var filter = Builders<Profile>.Filter.And(
            Builders<Profile>.Filter.Ne(p => p.Id, userProfile.Id),
            Builders<Profile>.Filter.Nin(p => p.Id, excludedProfileIds),
            Builders<Profile>.Filter.Eq(p => p.Gender, userProfile.PreferredGender),
            Builders<Profile>.Filter.Eq(p => p.PreferredGender, userProfile.Gender),
            Builders<Profile>.Filter.Gte(p => p.BirthDate, DateTime.Now.AddYears(-userProfile.AgeTo)),
            Builders<Profile>.Filter.Lte(p => p.BirthDate, DateTime.Now.AddYears(-userProfile.AgeFrom)),
            Builders<Profile>.Filter.Gte(p => p.AgeFrom, userProfile.AgeFrom),
            Builders<Profile>.Filter.Lte(p => p.AgeTo, userProfile.AgeTo)
        );

        if (locationAccessGranted && userProfile.Location != null)
        {
            var maxDistanceInMeters = userProfile.MaxDistance * 1000;
            var coordinates = userProfile.Location.Coordinates;
            var point = new GeoJsonPoint<GeoJson2DCoordinates>(coordinates);
            filter = filter & Builders<Profile>.Filter.NearSphere(p => p.Location, point, maxDistanceInMeters);
        }
        else
        {
            filter = filter & Builders<Profile>.Filter.Eq(p => p.Country, userProfile.Country);
        }

        return filter;
    }
}