using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations;

public class CityRepository(ProfileDbContext _dbContext)
    : GenericRepository<City, int>(_dbContext), ICityRepository
{
}