using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;

namespace Match.Domain.Repositories;

public interface ILikeRepository : IGenericRepository<Like, string>
{
    Task<Like?> CheckMutualLikeAsync(Like likeParam, CancellationToken cancellationToken);
}