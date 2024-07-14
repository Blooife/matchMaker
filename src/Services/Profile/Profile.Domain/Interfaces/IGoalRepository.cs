using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IGoalRepository : IGenericRepository<Goal, int>
{
    Task AddGoalToProfile(UserProfile profile, Goal goal, CancellationToken cancellationToken);
    Task RemoveGoalFromProfile(UserProfile profile, CancellationToken cancellationToken);
}