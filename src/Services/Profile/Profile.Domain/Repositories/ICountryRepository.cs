using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface ICountryRepository : IGenericRepository<Country, int>
{
    Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<List<City>> GetAllCitiesFromCountryAsync(int countryId, CancellationToken cancellationToken);
}
