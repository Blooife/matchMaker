using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class CountryRepository : GenericRepository<Country, int>, ICountryRepository
{
    private readonly ProfileDbContext _dbContext;
    public CountryRepository(ProfileDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Countries.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }

    public async Task<List<City>> GetAllCitiesFromCountryAsync(int countryId, CancellationToken cancellationToken)
    {
        var countryWithCities = await 
            _dbContext.Countries.Include(c => c.Cities).AsNoTracking().FirstOrDefaultAsync(c=>c.Id == countryId, cancellationToken);
        return countryWithCities!.Cities;
    }
}