using Match.Domain.Models;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;

namespace Match.Infrastructure.Repositories;

public class ChatRepository(IMongoCollection<Chat> _collection) : GenericRepository<Chat>(_collection), IChatRepository
{
    
}