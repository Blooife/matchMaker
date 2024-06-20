using Match.Domain.Models;
using Match.Domain.Repositories.BaseRepositories;

namespace Match.Domain.Repositories;

public interface ILikeRepository : IGenericRepository<Like>
{
    Task<bool> CheckMutualLike(Like likeParam, CancellationToken cancellationToken);
}