using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface IInterestRepository : IGenericRepository<Interest, int>
{
    Task<Interest?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task AddInterestToProfile(UserProfile profile, Interest interest);
    Task RemoveInterestFromProfile(UserProfile profile, Interest interest);
    Task<List<Interest>> GetUsersInterests(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetUserWithInterests(string profileId, CancellationToken cancellationToken);
}
