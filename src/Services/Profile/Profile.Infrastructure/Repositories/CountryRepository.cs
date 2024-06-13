using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class CountryRepository(ProfileDbContext _dbContext) : ICountryRepository
{
    public async Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Countries.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Country?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Countries.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
    
    public async Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Countries.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }

    public async Task<List<City>> GetAllCitiesFromCountryAsync(Country country, CancellationToken cancellationToken)
    {
        var countryWithCities = await 
            _dbContext.Countries.Include(c => c.Cities).AsNoTracking().FirstOrDefaultAsync(c=>c.Id == country.Id, cancellationToken);
        return countryWithCities!.Cities;
    }
}