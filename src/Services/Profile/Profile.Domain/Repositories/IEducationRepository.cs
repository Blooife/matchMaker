using Profile.Domain.Models;

namespace Profile.Domain.Repositories;

public interface IEducationRepository
{
    Task<IEnumerable<Education>> GetAllAsync(CancellationToken cancellationToken);
    Task<Education?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Education?> GetByNameAsync(string name, CancellationToken cancellationToken);
}
