using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IUserProfileRepository : IGenericRepository<UserProfile, string>
{
    Task<UserProfile> UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken);
    Task DeleteProfileAsync(UserProfile profile, CancellationToken cancellationToken);
    Task<UserProfile> CreateProfileAsync(UserProfile profile, CancellationToken cancellationToken);
    Task<IEnumerable<UserProfile>> GetAllProfileInfoByIdsAsync(IEnumerable<string> profileIds);
}