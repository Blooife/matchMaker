using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IImageRepository : IGenericRepository<Image, int>
{
    Task<Image> AddImageToProfileAsync(Image image, CancellationToken cancellationToken);
    Task RemoveImageFromProfileAsync(Image image, CancellationToken cancellationToken);
    Task<List<Image>> GetProfilesImagesAsync(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithImagesAsync(string profileId, CancellationToken cancellationToken);
}