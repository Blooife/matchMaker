using System.Linq.Expressions;

namespace Profile.Domain.Repositories.BaseRepositories;

public interface IGenericRepository<T, TKey> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    //Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    IQueryable<T> FindAll();
    Task<List<T>> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    Task<T?> FirstOrDefaultAsync(TKey id, CancellationToken cancellationToken);
}