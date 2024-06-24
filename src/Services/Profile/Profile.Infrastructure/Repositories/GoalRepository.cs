using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class GoalRepository : GenericRepository<Goal, int>, IGoalRepository
{
    private readonly ProfileDbContext _dbContext;
    public GoalRepository(ProfileDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddGoalToProfile(UserProfile profile, Goal goal, CancellationToken cancellationToken)
    {
        profile.GoalId = goal.Id;
        _dbContext.Entry(profile).State = EntityState.Modified;
    }
    
    public async Task RemoveGoalFromProfile(UserProfile profile, CancellationToken cancellationToken)
    {
        profile.GoalId = null;
        _dbContext.Entry(profile).State = EntityState.Modified;
    }
}