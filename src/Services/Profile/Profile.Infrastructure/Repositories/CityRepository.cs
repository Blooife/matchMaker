using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class CityRepository : GenericRepository<City, int>, ICityRepository
{
    private readonly ProfileDbContext _dbContext;
    public CityRepository(ProfileDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<City?> GetCityWithCountryById(int cityId, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities.Include(c => c.Country)
            .FirstOrDefaultAsync(c => c.Id == cityId, cancellationToken);
        
        return city;
    }
}