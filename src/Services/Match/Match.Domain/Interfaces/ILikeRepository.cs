using Match.Domain.Models;
using Match.Domain.Interfaces.BaseRepositories;

namespace Match.Domain.Interfaces;

public interface ILikeRepository : IGenericRepository<Like, string>
{
    Task<Like?> CheckMutualLikeAsync(Like likeParam, CancellationToken cancellationToken);
}