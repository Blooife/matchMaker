using System.Linq.Expressions;
using Match.Domain.Repositories.BaseRepositories;
using MongoDB.Driver;
using Shared.Models;

namespace Match.Infrastructure.Repositories.BaseRepositories;

public class GenericRepository<T, TKey>(IMongoCollection<T> _collection) : IGenericRepository<T, TKey> where T : class
{
    public void Create(T entity)
    {
        _collection.InsertOne(entity);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(entity, null, cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq("Id", GetIdValue(entity));
        await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq("Id", GetIdValue(entity));
        await _collection.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    {
        return await _collection.Find(condition).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync(cancellationToken);
    }

    private static object GetIdValue(T entity)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null)
        {
            throw new ArgumentException("Entity does not have an Id property");
        }
        return propertyInfo.GetValue(entity)!;
    }
}