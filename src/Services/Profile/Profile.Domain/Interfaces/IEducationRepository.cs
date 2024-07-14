using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IEducationRepository : IGenericRepository<Education, int>
{
    Task AddEducationToProfile(UserProfile profile, ProfileEducation userEducation);
    Task RemoveEducationFromProfile(UserProfile profile, ProfileEducation userEducation);
    Task UpdateProfilesEducation(ProfileEducation userEducation, string description);
    Task<List<ProfileEducation>> GetProfilesEducation(UserProfile profile, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithEducation(string profileId, CancellationToken cancellationToken);
}