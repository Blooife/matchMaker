using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IInterestRepository : IGenericRepository<Interest, int>
{
    Task AddInterestToProfileAsync(UserProfile profile, Interest interest);
    Task RemoveInterestFromProfileAsync(UserProfile profile, Interest interest, CancellationToken cancellationToken);
    Task<List<Interest>> GetProfilesInterestsAsync(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithInterestsAsync(string profileId, CancellationToken cancellationToken);
}
