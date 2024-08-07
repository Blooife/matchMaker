using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations;

public class InterestRepository(ProfileDbContext _dbContext)
    : GenericRepository<Interest, int>(_dbContext), IInterestRepository
{
    public async Task AddInterestToProfileAsync(UserProfile profile, Interest interest)
    {
        _dbContext.Profiles.Attach(profile);
        profile.Interests.Add(interest);
    }
    
    public async Task RemoveInterestFromProfileAsync(UserProfile profile, Interest interest, CancellationToken cancellationToken)
    {
        _dbContext.Profiles.Attach(profile);
        profile.Interests.Remove(interest);
    }
}