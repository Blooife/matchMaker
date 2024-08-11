using Profile.Domain.Models;
using Profile.Domain.Interfaces.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations.Repositories;

public class CityRepository(ProfileDbContext _dbContext)
    : GenericRepository<City, int>(_dbContext), ICityRepository
{
}