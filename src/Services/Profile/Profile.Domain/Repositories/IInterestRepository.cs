using Profile.Domain.Models;

namespace Profile.Domain.Repositories;

public interface IInterestRepository
{
    Task<IEnumerable<Interest>> GetAllAsync(CancellationToken cancellationToken);
    Task<Interest?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Interest?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task AddInterestToProfile(UserProfile profile, Interest interest, CancellationToken cancellationToken);
    Task RemoveInterestFromProfile(string profileId, int interestId, CancellationToken cancellationToken);
    Task<List<Interest>> GetUsersInterests(UserProfile profile, CancellationToken cancellationToken);
}
