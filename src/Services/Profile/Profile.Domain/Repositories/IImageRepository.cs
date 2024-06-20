using Profile.Domain.Models;
using Profile.Domain.Repositories.BaseRepositories;

namespace Profile.Domain.Repositories;

public interface IImageRepository : IGenericRepository<Image, int>
{
    Task<Image> AddImageToProfile(Image image, CancellationToken cancellationToken);
    Task RemoveImageFromProfile(Image image);
    Task<List<Image>> GetUsersImages(string profileId, CancellationToken cancellationToken);
    Task<UserProfile?> GetUserWithImages(string profileId, CancellationToken cancellationToken);
}