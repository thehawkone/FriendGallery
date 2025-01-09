using Application.DTO.Image;

namespace Application.Interfaces;

public interface IImageService
{
    Task UploadImageAsync(Guid userId, string imagePath);
    Task<List<ImageDto>> GetUserImagesAsync(Guid userId);
    Task<List<ImageDto>> GetFriendImagesAsync(Guid userId, Guid friendId);
}