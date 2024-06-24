using System.Linq.Expressions;
using Match.Domain.Models;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;
using Shared.Models;

namespace Match.Infrastructure.Repositories;

public class MessageRepository(IMongoCollection<Message> _collection) : GenericRepository<Message, string>(_collection), IMessageRepository
{
    public async Task<PagedList<Message>> GetPagedAsync(string chatId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        Expression<Func<Message, bool>> filter = x => x.ChatId == chatId;

        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<Message, Message>()
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize,
            Sort = Builders<Message>.Sort.Descending(m => m.Timestamp)
        };

        var items = await _collection.Find(filter)
            .Sort(findOptions.Sort)
            .Skip(findOptions.Skip)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        return new PagedList<Message>(items, (int)count, pageNumber, pageSize);
    }

}