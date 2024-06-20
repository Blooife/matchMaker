using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface ICountryRepository : IGenericRepository<Country, int>
{
    Task<List<City>> GetAllCitiesFromCountryAsync(int countryId, CancellationToken cancellationToken);
}