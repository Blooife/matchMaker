using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations;

public class GoalRepository(ProfileDbContext _dbContext)
    : GenericRepository<Goal, int>(_dbContext), IGoalRepository
{

}