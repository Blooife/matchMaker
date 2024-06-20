using Match.Domain.Models;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;

namespace Match.Infrastructure.Repositories;

public class MessageRepository(IMongoCollection<Message> _collection) : GenericRepository<Message>(_collection), IMessageRepository
{
    
}