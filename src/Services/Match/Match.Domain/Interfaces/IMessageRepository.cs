using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;

namespace Match.Domain.Interfaces;

public interface IMessageRepository : IGenericRepository<Message, string>
{
    Task<(List<Message>, int)> GetPagedAsync(string chatId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}