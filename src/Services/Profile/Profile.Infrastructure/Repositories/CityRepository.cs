using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class CityRepository(ProfileDbContext _dbContext)
    : GenericRepository<City, int>(_dbContext), ICityRepository
{
    public async Task<City?> GetCityWithCountryByIdAsync(int cityId, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities.Include(c => c.Country)
            .FirstOrDefaultAsync(c => c.Id == cityId, cancellationToken);
        
        return city;
    }
}