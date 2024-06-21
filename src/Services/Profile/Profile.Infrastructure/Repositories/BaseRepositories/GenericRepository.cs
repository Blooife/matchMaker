using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Profile.Domain.Repositories.BaseRepositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories.BaseRepositories;

public class GenericRepository<T, TKey>(ProfileDbContext _dbContext) : IGenericRepository<T, TKey> where T : class
{
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
    }
    
    public async Task<T?> FirstOrDefaultAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id), cancellationToken);
    }
    
    public async Task<bool> Exists(TKey id, CancellationToken cancellationToken) //isExist
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        return entity != null;
    }
    
    public IQueryable<T> FindAll()
    {
        return _dbContext.Set<T>();
    }
    
    public Task<List<T>> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        return FindAll().Where(expression).AsNoTracking().ToListAsync(cancellationToken);
    }
}