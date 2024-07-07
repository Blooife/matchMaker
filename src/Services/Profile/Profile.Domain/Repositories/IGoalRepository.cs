using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface IGoalRepository : IGenericRepository<Goal, int>
{
}