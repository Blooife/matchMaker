using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces.Repositories;

public interface IEducationRepository : IGenericRepository<Education, int>
{
    Task AddEducationToProfileAsync(UserProfile profile, ProfileEducation userEducation);
    Task RemoveEducationFromProfileAsync(UserProfile profile, ProfileEducation userEducation);
    Task UpdateProfilesEducationAsync(ProfileEducation userEducation, string description);
}