using Match.Domain.Models;
using Match.Domain.Interfaces;
using Match.Infrastructure.Implementations.BaseRepositories;
using MongoDB.Driver;
using Shared.Constants;

namespace Match.Infrastructure.Implementations;

public class ProfileRepository(IMongoCollection<Profile> _collection) : GenericRepository<Profile, string>(_collection), IProfileRepository
{
    public async Task<List<string>> GetRecsAsync(List<string> excludedProfileIds, Profile userProfile, CancellationToken cancellationToken)
    {
        var filter = GetFilterForRecommendations(excludedProfileIds, userProfile);
        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<Profile, Profile>()
        {
            Limit = 10,
            Projection = Builders<Profile>.Projection.Include(p => p.Id)
        };

        var ids = await _collection.Find(filter)
            .Project(p=>p.Id)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        return ids;
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

        if (userProfile.AgeFrom != 0 && userProfile.AgeTo != 0)
        {
            filters.Add(Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Gte(p => p.BirthDate, DateTime.Now.AddYears(-userProfile.AgeTo)),
                Builders<Profile>.Filter.Lte(p => p.BirthDate, DateTime.Now.AddYears(-userProfile.AgeFrom))
            ));
        }

        if (userProfile is { Location: not null, MaxDistance: > 0 })
        {
            var coordinates = userProfile.Location.Coordinates;
            filters.Add(Builders<Profile>.Filter.GeoWithinCenterSphere(p=>p.Location, coordinates.X, coordinates.Y, userProfile.MaxDistance / 6371.0 ));
        }
        else
        {
            filters.Add(Builders<Profile>.Filter.Eq(p => p.Country, userProfile.Country));
        }
        
        var resFilter = ApplySoftDeleteFilter(Builders<Profile>.Filter.And(filters));
        
        return resFilter;
    }
}