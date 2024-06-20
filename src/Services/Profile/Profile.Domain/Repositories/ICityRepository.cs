using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface ICityRepository : IGenericRepository<City, int>
{
    Task AddCityToProfile(UserProfile profile, City city, CancellationToken cancellationToken);
    Task RemoveCityFromProfile(UserProfile profile, CancellationToken cancellationToken);
    Task<City?> GetCityWithCountryById(int cityId, CancellationToken cancellationToken);
}