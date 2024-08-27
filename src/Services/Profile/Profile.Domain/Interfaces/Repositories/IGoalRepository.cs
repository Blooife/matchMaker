using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces.Repositories;

public interface IGoalRepository : IGenericRepository<Goal, int>;