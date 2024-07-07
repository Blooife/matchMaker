using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class ImageRepository(ProfileDbContext _dbContext, ICacheService _cacheService)
    : GenericRepository<Image, int>(_dbContext, _cacheService), IImageRepository
{
    private readonly string _cacheKeyPrefix = typeof(Image).Name;
    
    public async Task<Image> AddImageToProfile(Image image, CancellationToken cancellationToken)
    {
        await _dbContext.Images.AddAsync(image, cancellationToken);
        
        return image;
    }
    
    public async Task RemoveImageFromProfile(Image image, CancellationToken cancellationToken)
    {
        _dbContext.Images.Remove(image);
        
        var cacheKey = $"{_cacheKeyPrefix}_{image.Id}";
        await _cacheService.RemoveAsync(cacheKey, cancellationToken);
    }

    public async Task<List<Image>> GetProfilesImages(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Images).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile!.Images;
    }
    
    public async Task<UserProfile?> GetProfileWithImages(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}