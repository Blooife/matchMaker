using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class ImageRepository(ProfileDbContext _dbContext)
    : GenericRepository<Image, int>(_dbContext), IImageRepository
{
    public async Task<Image> AddImageToProfileAsync(Image image, CancellationToken cancellationToken)
    {
        await _dbContext.Images.AddAsync(image, cancellationToken);
        
        return image;
    }
    
    public async Task RemoveImageFromProfileAsync(Image image, CancellationToken cancellationToken)
    {
        _dbContext.Images.Remove(image);
    }

    public async Task<List<Image>> GetProfilesImagesAsync(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Images).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile!.Images;
    }
    
    public async Task<UserProfile?> GetProfileWithImagesAsync(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}