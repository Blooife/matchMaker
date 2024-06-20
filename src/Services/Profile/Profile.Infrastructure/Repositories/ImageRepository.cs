using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Repositories.BaseRepositories;

namespace Profile.Infrastructure.Repositories;

public class ImageRepository : GenericRepository<Image, int>, IImageRepository
{
    private readonly ProfileDbContext _dbContext;
    
    public ImageRepository(ProfileDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Image> AddImageToProfile(Image image, CancellationToken cancellationToken)
    {
        await _dbContext.Images.AddAsync(image, cancellationToken);
        return image;
        //profile.Images.Add(image);
    }
    
    public async Task RemoveImageFromProfile(Image image)
    {
        _dbContext.Images.Remove(image);
        //profile.Images.Remove(image);
    }

    public async Task<List<Image>> GetUsersImages(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Images).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile!.Images;
    }
    
    public async Task<UserProfile?> GetUserWithImages(string profileId, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.Profiles.Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        return userProfile;
    }
}