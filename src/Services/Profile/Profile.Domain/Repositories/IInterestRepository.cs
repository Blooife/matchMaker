using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface IInterestRepository : IGenericRepository<Interest, int>
{
    Task AddInterestToProfile(UserProfile profile, Interest interest);
    Task RemoveInterestFromProfile(UserProfile profile, Interest interest, CancellationToken cancellationToken);
    Task<List<Interest>> GetProfilesInterests(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithInterests(string profileId, CancellationToken cancellationToken);
}
