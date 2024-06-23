using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;
using Shared.Models;

namespace Match.Domain.Repositories;

public interface IMessageRepository : IGenericRepository<Message, int>
{
    Task<PagedList<Message>> GetPagedAsync(int chatId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}