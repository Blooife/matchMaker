using MongoDB.Driver;

namespace Match.Infrastructure.Context;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
}