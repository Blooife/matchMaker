using Match.Domain.Models;
using Match.Domain.Interfaces;
using Match.Infrastructure.Implementations.BaseRepositories;
using MongoDB.Driver;

namespace Match.Infrastructure.Implementations;

public class LikeRepository(IMongoCollection<Like> _collection) : GenericRepository<Like, string>(_collection), ILikeRepository
{
    public async Task<Like?> CheckMutualLikeAsync(Like likeParam, CancellationToken cancellationToken)
    {
        var getResult = await GetAsync(like =>
            like.IsLike && like.ProfileId == likeParam.TargetProfileId && like.TargetProfileId == likeParam.ProfileId, cancellationToken);
        return getResult.FirstOrDefault();
    }
}