using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class GoalRepository(ProfileDbContext _dbContext, ICacheService _cacheService)
    : GenericRepository<Goal, int>(_dbContext, _cacheService), IGoalRepository
{

}