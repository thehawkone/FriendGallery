using Domain.Models;

namespace Domain.Abstractions;

public interface IImageRepository
{
    Task AddImageAsync(Image image);
    Task<IEnumerable<Image>> GetImagesByUserIdAsync(Guid userId);
    Task UpdateImageAsync(Image image);
    Task DeleteImageAsync(Image image);
}