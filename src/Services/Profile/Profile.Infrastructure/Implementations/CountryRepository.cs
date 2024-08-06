using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations;

public class CountryRepository(ProfileDbContext _dbContext)
    : GenericRepository<Country, int>(_dbContext), ICountryRepository
{
    public async Task<List<City>> GetAllCitiesFromCountryAsync(int countryId, CancellationToken cancellationToken)
    {
        var countryWithCities = await 
            _dbContext.Countries.Include(c => c.Cities).AsNoTracking()
                .FirstOrDefaultAsync(c=>c.Id == countryId, cancellationToken);
        
        return countryWithCities!.Cities;
    }
}