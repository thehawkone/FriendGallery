using Domain.Models;

namespace Domain.Abstractions;

public interface IImageRepository
{
    Task AddImageAsync(Image image);
    Task<Image> GetImageAsync(Image image);
    Task UpdateImageAsync(Image image);
    Task DeleteImageAsync(Image image);
}