using Match.Domain.Models;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;

namespace Match.Infrastructure.Repositories;

public class ChatRepository(IMongoCollection<Chat> _collection) : GenericRepository<Chat, int>(_collection), IChatRepository
{
    public async Task<IEnumerable<Chat>> GetChatsByProfileId(string profileId, CancellationToken cancellationToken)
    {
        var chats = await GetAsync(chat => chat.ProfileId1 == profileId || chat.ProfileId2 == profileId, cancellationToken);
        return chats;
    }
    
    //to do getPaged order by last message
}