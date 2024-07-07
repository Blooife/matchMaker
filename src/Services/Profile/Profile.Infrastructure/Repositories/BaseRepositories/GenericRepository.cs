using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Profile.Domain.Repositories.BaseRepositories;
using Profile.Domain.Specifications;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;

namespace Profile.Infrastructure.Repositories.BaseRepositories;

public class GenericRepository<T, TKey>(ProfileDbContext _dbContext, ICacheService _cacheService) : IGenericRepository<T, TKey> where T : class
{
    private readonly string _cacheKeyPrefix = typeof(T).Name;
    
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        var data = await _dbContext.Set<T>().WhereNotDeleted().AsNoTracking().ToListAsync(cancellationToken);

        foreach (var entity in data)
        {
            var idValue = GetIdValue(entity);
            var cacheKey = $"{_cacheKeyPrefix}_{idValue}";
            await _cacheService.SetAsync(cacheKey, entity, cancellationToken);
        }

        return data;
    }
    
    public async Task<T?> FirstOrDefaultAsync(TKey id, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}_{id}";
        var cachedData = await _cacheService.GetAsync<T>(cacheKey, cancellationToken);
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var data = await _dbContext.Set<T>().WhereNotDeleted().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id), cancellationToken);
        if (data is not null)
        {
            await _cacheService.SetAsync(cacheKey, data, cancellationToken);
        }

        return data;
    }
    
    public IQueryable<T> FindAll()
    {
        return _dbContext.Set<T>().WhereNotDeleted().AsNoTracking();
    }
    
    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        var data = await FindAll().WhereNotDeleted().Where(expression).AsNoTracking().ToListAsync(cancellationToken);
        
        foreach (var entity in data)
        {
            var idValue = GetIdValue(entity);
            var cacheKey = $"{_cacheKeyPrefix}_{idValue}";
            await _cacheService.SetAsync(cacheKey, entity, cancellationToken);
        }

        return data;
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