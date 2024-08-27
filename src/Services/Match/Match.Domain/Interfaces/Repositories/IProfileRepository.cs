using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;

namespace Match.Domain.Interfaces.Repositories;

public interface IProfileRepository : IGenericRepository<Profile, string>
{
    Task<List<string>> GetRecsAsync(List<string> excludedProfileIds, Profile userProfile, CancellationToken cancellationToken);
}