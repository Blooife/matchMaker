using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces.Repositories;

public interface ICityRepository : IGenericRepository<City, int>;