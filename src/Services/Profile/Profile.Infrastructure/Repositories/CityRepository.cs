using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class CityRepository(ProfileDbContext _dbContext, ICacheService _cacheService)
    : GenericRepository<City, int>(_dbContext, _cacheService), ICityRepository
{
    public async Task<City?> GetCityWithCountryById(int cityId, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities.Include(c => c.Country)
            .FirstOrDefaultAsync(c => c.Id == cityId, cancellationToken);
        
        return city;
    }
}