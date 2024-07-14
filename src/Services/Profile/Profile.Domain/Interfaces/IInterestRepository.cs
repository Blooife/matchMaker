using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IInterestRepository : IGenericRepository<Interest, int>
{
    Task AddInterestToProfile(UserProfile profile, Interest interest);
    Task RemoveInterestFromProfile(UserProfile profile, Interest interest);
    Task<List<Interest>> GetProfilesInterests(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithInterests(string profileId, CancellationToken cancellationToken);
}
