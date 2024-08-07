using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;

namespace Match.Domain.Interfaces;

public interface IChatRepository : IGenericRepository<Chat, string>
{
    Task<IEnumerable<Chat>> GetChatsByProfileIdAsync(string profileId, CancellationToken cancellationToken);
    Task<(List<Chat>, int)> GetPagedAsync(string profileId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
    Task<Chat?> GetChatByProfilesIdsAsync(string firstProfileId, string secondProfileId, CancellationToken cancellationToken);
}