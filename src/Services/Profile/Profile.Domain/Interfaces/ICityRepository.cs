using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface ICityRepository : IGenericRepository<City, int>
{
    Task<City?> GetCityWithCountryByIdAsync(int cityId, CancellationToken cancellationToken);
}