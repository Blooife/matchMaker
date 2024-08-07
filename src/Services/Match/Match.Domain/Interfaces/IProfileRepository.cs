using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;

namespace Match.Domain.Interfaces;

public interface IProfileRepository : IGenericRepository<Profile, string>
{
    Task<List<string>> GetRecsAsync(List<string> excludedProfileIds, Profile userProfile, CancellationToken cancellationToken);
}