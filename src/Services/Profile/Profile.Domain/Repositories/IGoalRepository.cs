using Profile.Domain.Models;

namespace Profile.Domain.Repositories;

public interface IGoalRepository
{
    Task<IEnumerable<Goal>> GetAllAsync(CancellationToken cancellationToken);
    Task<Goal?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Goal?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task AddGoalToProfile(UserProfile profile, Goal goal, CancellationToken cancellationToken);
    Task RemoveGoalFromProfile(UserProfile profile, CancellationToken cancellationToken);

}