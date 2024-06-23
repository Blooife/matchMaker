using System.Linq.Expressions;

namespace Match.Domain.Repositories.BaseRepositories;

public interface IGenericRepository<T, TKey> where T : class
{
    void Create(T entity);
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
    Task<List<T>> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
}