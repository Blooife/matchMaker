using Profile.Domain.Models;
using Profile.Domain.Interfaces.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.BaseRepositories;

namespace Profile.Infrastructure.Implementations.Repositories;

public class ImageRepository(ProfileDbContext _dbContext)
    : GenericRepository<Image, int>(_dbContext), IImageRepository
{
    public async Task<Image> AddImageToProfileAsync(Image image, CancellationToken cancellationToken)
    {
        await _dbContext.Images.AddAsync(image, cancellationToken);
        
        return image;
    }
    
    public async Task RemoveImageFromProfileAsync(Image image)
    {
        _dbContext.Images.Remove(image);
    }
    
    public async Task UpdateImageAsync(Image image)
    {
        _dbContext.Images.Update(image);
    }
}