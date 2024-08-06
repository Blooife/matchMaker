using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface ICountryRepository : IGenericRepository<Country, int>
{
    Task<List<City>> GetAllCitiesFromCountryAsync(int countryId, CancellationToken cancellationToken);
}