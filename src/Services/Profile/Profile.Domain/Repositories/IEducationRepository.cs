using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface IEducationRepository : IGenericRepository<Education, int>
{
    Task<Education?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task AddEducationToProfile(UserProfile profile, UserEducation userEducation);

    Task RemoveEducationFromProfile(UserProfile profile, UserEducation userEducation);
    Task UpdateUsersEducation(UserEducation userEducation, string description);
    Task<List<UserEducation>> GetUsersEducation(UserProfile profile, CancellationToken cancellationToken);
    Task<UserProfile?> GetUserWithEducation(string profileId, CancellationToken cancellationToken);
}
