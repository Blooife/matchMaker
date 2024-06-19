using Match.Domain.Models;
using Match.Domain.Repositories;
using Match.Infrastructure.Repositories.BaseRepositories;
using MongoDB.Driver;

namespace Match.Infrastructure.Repositories;

public class ProfileRepository(IMongoCollection<Profile> _collection) : GenericRepository<Profile>(_collection), IProfileRepository
{
    
}