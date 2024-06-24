using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;
using Shared.Models;

namespace Match.Domain.Repositories;

public interface IChatRepository : IGenericRepository<Chat, string>
{
    Task<IEnumerable<Chat>> GetChatsByProfileId(string profileId, CancellationToken cancellationToken);

    Task<PagedList<Chat>> GetPagedAsync(string profileId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}