using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;

namespace Match.Domain.Repositories;

public interface ILikeRepository : IGenericRepository<Like, string>
{
    Task<Like?> CheckMutualLike(Like likeParam, CancellationToken cancellationToken);
}