using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;
using Shared.Models;

namespace Match.Domain.Interfaces;

public interface IChatRepository : IGenericRepository<Chat, string>
{
    Task<IEnumerable<Chat>> GetChatsByProfileIdAsync(string profileId, CancellationToken cancellationToken);
    Task<PagedList<Chat>> GetPagedAsync(string profileId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}