using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface ICityRepository : IGenericRepository<City, int>
{
    Task<City?> GetCityWithCountryByIdAsync(int cityId, CancellationToken cancellationToken);
}