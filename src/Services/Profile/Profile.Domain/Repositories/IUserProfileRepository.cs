using Profile.Domain.Models;

namespace Profile.Domain.Repositories;

public interface IUserProfileRepository
{
    Task<IEnumerable<UserProfile>> GetAllProfilesAsync(CancellationToken cancellationToken);
    Task<UserProfile> UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken);
    Task DeleteProfileAsync(UserProfile profile, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileByIdAsync(string profileId, CancellationToken cancellationToken);
    Task<UserProfile> CreateProfileAsync(UserProfile profile, CancellationToken cancellationToken);
}