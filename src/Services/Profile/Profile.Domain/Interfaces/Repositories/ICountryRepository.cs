using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces.Repositories;

public interface ICountryRepository : IGenericRepository<Country, int>
{
    Task<List<City>> GetAllCitiesFromCountryAsync(int countryId, CancellationToken cancellationToken);
}