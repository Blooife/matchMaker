using Profile.Domain.Models;

namespace Profile.Domain.Repositories;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken);
    Task<City?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<City?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task AddCityToProfile(UserProfile profile, City city, CancellationToken cancellationToken);
    Task RemoveCityFromProfile(UserProfile profile, CancellationToken cancellationToken);
}