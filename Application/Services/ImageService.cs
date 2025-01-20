using Application.DTO.Image;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class ImageService : IImageService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IImageRepository _imageRepository;

    public ImageService(IUserRepository userRepository, IImageRepository imageRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
        _configuration = configuration;
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
        
        var storagePath = _configuration["StorageSettings:ImagePath"];
        if (string.IsNullOrWhiteSpace(storagePath)) {
            throw new InvalidOperationException("Путь к хранилищу изображений не задан в appsettings.json.");
        }
        
        if (!Directory.Exists(storagePath)) {
            Directory.CreateDirectory(storagePath);
        }
        
        var filePath = Path.Combine(storagePath, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create)) {
            await imageFile.CopyToAsync(fileStream);
        }
        
        var image = new Image
        {
            PhotoId = Guid.NewGuid(),
            FilePath = fileName,
            FileName = fileName,
            FileSize = imageFile.Length,
            ContentType = imageFile.ContentType,
            UserId = user.UserId,
            User = user
        };
        
        user.AddImage(image);
        await _imageRepository.AddImageAsync(image);
    }

    public async Task<IEnumerable<string>> GetUserImagesAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) {
            throw new ArgumentException("Пользователь не найден");
        }
        
        var images = await _imageRepository.GetImagesByUserIdAsync(userId);
        var storagePath = _configuration["StorageSettings:ImagePath"];
        if (string.IsNullOrWhiteSpace(storagePath))
        {
            throw new InvalidOperationException("Путь к хранилищу изображений не задан в appsettings.json.");
        }
        
        return images.Select(i => i.FilePath).ToList();
    }
}