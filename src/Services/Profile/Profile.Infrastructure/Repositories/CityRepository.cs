using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class CityRepository(ProfileDbContext _dbContext) : ICityRepository
{
    public async Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Cities.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<City?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Cities.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
    
    public async Task<City?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Cities.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }
    
    public async Task AddCityToProfile(UserProfile profile, City city, CancellationToken cancellationToken)
    {
        profile.CityId = city.Id;
        _dbContext.Entry(profile).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveCityFromProfile(UserProfile profile, CancellationToken cancellationToken)
    {
        profile.CityId = null;
        _dbContext.Entry(profile).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}