using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;

namespace Match.Domain.Repositories;

public interface IChatRepository : IGenericRepository<Chat, int>
{
    Task<IEnumerable<Chat>> GetChatsByProfileId(string profileId, CancellationToken cancellationToken);
}