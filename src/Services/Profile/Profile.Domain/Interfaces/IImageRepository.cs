using Profile.Domain.Models;
using Profile.Domain.Interfaces.BaseRepositories;

namespace Profile.Domain.Interfaces;

public interface IImageRepository : IGenericRepository<Image, int>
{
    Task<Image> AddImageToProfileAsync(Image image, CancellationToken cancellationToken);
    Task RemoveImageFromProfileAsync(Image image, CancellationToken cancellationToken);
    Task UpdateImageAsync(Image image, CancellationToken cancellationToken);
}