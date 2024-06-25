using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;
using Shared.Models;

namespace Match.Domain.Repositories;

public interface IMessageRepository : IGenericRepository<Message, string>
{
    Task<PagedList<Message>> GetPagedAsync(string chatId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);

    Task DeleteMessagesByChatId(string chatId, CancellationToken cancellationToken);
}