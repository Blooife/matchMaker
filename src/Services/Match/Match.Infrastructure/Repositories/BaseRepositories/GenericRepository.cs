using System.Linq.Expressions;
using Match.Domain.Repositories.BaseRepositories;
using MongoDB.Driver;

namespace Match.Infrastructure.Repositories.BaseRepositories;

public class GenericRepository<T>(IMongoCollection<T> _collection) : IGenericRepository<T> where T : class
{
    public void Create(T entity)
    {
        
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken)
    {
        
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        
    }

    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    {
        //throw new NotImplementedException();
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        //throw new NotImplementedException();
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        //throw new NotImplementedException();    
    }
}