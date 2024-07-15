using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IImageRepository : IGenericRepository<Image, int>
{
    Task<Image> AddImageToProfile(Image image, CancellationToken cancellationToken);
    Task RemoveImageFromProfile(Image image);
    Task<List<Image>> GetProfilesImages(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetProfileWithImages(string profileId, CancellationToken cancellationToken);
}