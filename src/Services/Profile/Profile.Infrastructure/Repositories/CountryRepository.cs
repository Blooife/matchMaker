using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class CountryRepository(ProfileDbContext _dbContext, ICacheService _cacheService)
    : GenericRepository<Country, int>(_dbContext, _cacheService), ICountryRepository
{
    public async Task<List<City>> GetAllCitiesFromCountryAsync(int countryId, CancellationToken cancellationToken)
    {
        var countryWithCities = await 
            _dbContext.Countries.Include(c => c.Cities).AsNoTracking()
                .FirstOrDefaultAsync(c=>c.Id == countryId, cancellationToken);
        
        return countryWithCities!.Cities;
    }
}