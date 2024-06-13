using Profile.Domain.Models;

namespace Profile.Domain.Repositories;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken);
    Task<Country?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<List<City>> GetAllCitiesFromCountryAsync(Country country, CancellationToken cancellationToken);
}
