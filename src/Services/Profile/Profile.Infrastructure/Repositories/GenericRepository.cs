using Microsoft.EntityFrameworkCore;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> 
    where T : class
    where TKey : struct, System.IConvertible
{
    private readonly ProfileDbContext _dbContext;
    
    public GenericRepository(ProfileDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<bool> Exists(TKey id, CancellationToken cancellationToken)
    {
        var entity = await Get(id, cancellationToken);
        return entity != null;
    }
    
    public async Task<T?> Get(TKey id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Set<T>().Remove(entity);
    }
}