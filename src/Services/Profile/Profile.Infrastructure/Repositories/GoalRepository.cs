using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class GoalRepository(ProfileDbContext _dbContext) : IGoalRepository
{
    public async Task<IEnumerable<Goal>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Goals.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Goal?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Goals.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
    
    public async Task<Goal?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Goals.AsNoTracking().FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
    }
    
    public async Task AddGoalToProfile(UserProfile profile, Goal goal, CancellationToken cancellationToken)
    {
        profile.GoalId = goal.Id;
        _dbContext.Entry(profile).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveGoalFromProfile(UserProfile profile, CancellationToken cancellationToken)
    {
        profile.GoalId = null;
        _dbContext.Entry(profile).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}