using Application.DTO.Image;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class ImageService : IImageService
{
    private readonly IUserRepository _userRepository;
    private readonly IImageRepository _imageRepository;

    public ImageService(IUserRepository userRepository, IImageRepository imageRepository)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
    }
    
    public async Task UploadImageAsync(Guid userId, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0) {
            throw new ArgumentException("Файл отсутствует или пустой", nameof(imageFile));
        }
        
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) {
            throw new ArgumentException("Пользователь не найден");
        }
        
        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(@"C:\Users\Administrator\Desktop\Images", fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create)) {
            await imageFile.CopyToAsync(fileStream);
        }
        
        var image = new Image
        {
            PhotoId = Guid.NewGuid(),
            FilePath = filePath,
            FileName = fileName,
            FileSize = imageFile.Length,
            ContentType = imageFile.ContentType,
            UserId = user.UserId,
            User = user
        };
        
        user.AddImage(image);
        await _imageRepository.AddImageAsync(image);
    }
}