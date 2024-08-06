using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;
using Shared.Models;

namespace Match.Domain.Interfaces;

public interface IMessageRepository : IGenericRepository<Message, string>
{
    Task<PagedList<Message>> GetPagedAsync(string chatId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}