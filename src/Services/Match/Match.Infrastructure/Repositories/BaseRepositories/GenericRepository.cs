using System.Linq.Expressions;
using Match.Domain.Repositories.BaseRepositories;
using MongoDB.Driver;
using Shared.Interfaces;

namespace Match.Infrastructure.Repositories.BaseRepositories;

public class GenericRepository<T, TKey>(IMongoCollection<T> _collection) : IGenericRepository<T, TKey> where T : class, ISoftDeletable
{
    private readonly FilterDefinition<T> _notDeletedFilter = Builders<T>.Filter.Eq(e => e.DeletedAt, null);
    
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
        entity.DeletedAt = DateTime.UtcNow;
        await UpdateAsync(entity, cancellationToken);
    }

    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    {
        return await _collection.Find(condition & _notDeletedFilter).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        
        return await _collection.Find(filter & _notDeletedFilter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _collection.Find(_notDeletedFilter).ToListAsync(cancellationToken);
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