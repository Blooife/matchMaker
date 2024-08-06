using System.Linq.Expressions;
using Match.Domain.Models;
using Match.Domain.Interfaces;
using Match.Infrastructure.Implementations.BaseRepositories;
using MongoDB.Driver;
using Shared.Models;

namespace Match.Infrastructure.Implementations;

public class ChatRepository(IMongoCollection<Chat> _collection) : GenericRepository<Chat, string>(_collection), IChatRepository
{
    public async Task<IEnumerable<Chat>> GetChatsByProfileIdAsync(string profileId, CancellationToken cancellationToken)
    {
        var chats = await GetAsync(chat => chat.FirstProfileId == profileId || chat.SecondProfileId == profileId, cancellationToken);
        return chats;
    }
    
    public async Task<PagedList<Chat>> GetPagedAsync(string profileId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        Expression<Func<Chat, bool>> filter = chat => chat.FirstProfileId == profileId || chat.SecondProfileId == profileId;

        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<Chat, Chat>()
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize,
            Sort = Builders<Chat>.Sort.Descending(chat => chat.LastMessageTimestamp)
        };

        var items = await _collection.Find(filter)
            .Sort(findOptions.Sort)
            .Skip(findOptions.Skip)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        return new PagedList<Chat>(items, (int)count, pageNumber, pageSize);
    }
}