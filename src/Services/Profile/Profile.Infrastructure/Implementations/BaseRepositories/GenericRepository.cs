using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Profile.Domain.Interfaces.BaseRepositories;
using Profile.Domain.Specifications;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Implementations.BaseRepositories;

public class GenericRepository<T, TKey>(ProfileDbContext _dbContext) : IGenericRepository<T, TKey> where T : class
{
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().WhereNotDeleted().AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<T?> FirstOrDefaultAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().WhereNotDeleted().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id), cancellationToken);
    }
    
    public IQueryable<T> FindAll()
    {
        return _dbContext.Set<T>().WhereNotDeleted().AsNoTracking();
    }
    
    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        return await FindAll().WhereNotDeleted().Where(expression).AsNoTracking().ToListAsync(cancellationToken);
    }
}