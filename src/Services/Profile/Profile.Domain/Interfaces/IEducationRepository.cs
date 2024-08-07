using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IEducationRepository : IGenericRepository<Education, int>
{
    Task AddEducationToProfileAsync(UserProfile profile, ProfileEducation userEducation);
    Task RemoveEducationFromProfileAsync(UserProfile profile, ProfileEducation userEducation);
    Task UpdateProfilesEducationAsync(ProfileEducation userEducation, string description);
    Task<List<ProfileEducation>> GetProfilesEducationAsync(UserProfile profile, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithEducationAsync(string profileId, CancellationToken cancellationToken);
}