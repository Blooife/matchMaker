using System.Linq.Expressions;
using Match.Domain.Interfaces.BaseRepositories;
using MongoDB.Driver;
using Shared.Interfaces;

namespace Match.Infrastructure.Implementations.BaseRepositories;

public class GenericRepository<T, TKey>(IMongoCollection<T> _collection) : IGenericRepository<T, TKey> where T : class
{
    private FilterDefinition<T> ApplySoftDeleteFilter(FilterDefinition<T> baseFilter)
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
        {
            var builder = Builders<T>.Filter;
            var softDeleteFilter = builder.Eq(e => ((ISoftDeletable)e).DeletedAt, null);
            return builder.And(baseFilter, softDeleteFilter);
        }

        return baseFilter;
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
        if (entity is ISoftDeletable deletableEntity)
        {
            deletableEntity.DeletedAt = DateTime.UtcNow;
            await UpdateAsync(entity, cancellationToken);
        }
        else
        {
            var filter = Builders<T>.Filter.Eq("Id", GetIdValue(entity));
            await _collection.DeleteOneAsync(filter, cancellationToken);
        }
    }
    
    public async Task DeleteManyAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
        {
            var update = Builders<T>.Update.Set(e => ((ISoftDeletable)e).DeletedAt, DateTime.UtcNow);
            await _collection.UpdateManyAsync(condition, update, cancellationToken: cancellationToken);
        }
        else
        {
            var filter = Builders<T>.Filter.Where(condition);
            await _collection.DeleteManyAsync(filter, cancellationToken);
        }
    }

    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    {
        var baseFilter = Builders<T>.Filter.Where(condition);
        var filter = ApplySoftDeleteFilter(baseFilter);
        
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        var baseFilter = Builders<T>.Filter.Eq("Id", id);
        var filter = ApplySoftDeleteFilter(baseFilter);
        
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        var baseFilter = Builders<T>.Filter.Empty;
        var filter = ApplySoftDeleteFilter(baseFilter);
        return await _collection.Find(filter).ToListAsync(cancellationToken);
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