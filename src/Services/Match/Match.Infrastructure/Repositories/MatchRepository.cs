using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;
using Match.Domain.Models;

namespace Match.Infrastructure.Repositories;

public class MatchRepository(IMongoCollection<CoupleMatch> _collection) : GenericRepository<CoupleMatch>(_collection), IMatchRepository
{
    
}