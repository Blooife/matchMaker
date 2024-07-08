using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class GoalRepository(ProfileDbContext _dbContext)
    : GenericRepository<Goal, int>(_dbContext), IGoalRepository
{

}