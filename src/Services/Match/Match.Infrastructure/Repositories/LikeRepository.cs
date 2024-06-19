using Match.Domain.Models;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;

namespace Match.Infrastructure.Repositories;

public class LikeRepository(IMongoCollection<Like> _collection) : GenericRepository<Like>(_collection), ILikeRepository
{
    
}