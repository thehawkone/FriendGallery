using Application.DTO.Image;
using Domain.Abstractions;
using Domain.Models;

namespace Application.Services;

public class ImageService 
{
    private readonly IUserRepository _userRepository;
    private readonly IImageRepository _imageRepository;

    public ImageService(IUserRepository userRepository, IImageRepository imageRepository)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
    }

    public async Task UploadImageAsync(Guid userId, string imagePath)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) {
            throw new Exception("Пользователь не найден");
        }
        
        var image = new Image { PhotoId = Guid.NewGuid(), FilePath = imagePath , UserId = userId, User = user};
        user.AddImage(image);

        await _imageRepository.AddImageAsync(image);
    }

    public async Task<List<ImageDto>> GetUserImagesAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) {
            throw new Exception("Пользователь не найден");
        }

        return user.Images.Select(i => new ImageDto
        {
            PhotoId = i.PhotoId,
            FilePath = i.FilePath,
        }).ToList();
    }

    public async Task<List<ImageDto>> GetFriendImagesAsync(Guid userId, Guid friendId)
    {
        var user = await _userRepository.GetUserWithFriendsAsync(userId);
        if (user == null) {
            throw new Exception("Пользователь не найден");
        }

        var friend = user.Friends.FirstOrDefault(f => f.FriendId == friendId)!.Friend;
        if (friend == null) {
            throw new UnauthorizedAccessException("Доступ к фотографиям друга запрещён!");
        }

        return friend.Images.Select(i => new ImageDto
        {
            PhotoId = i.PhotoId,
            FilePath = i.FilePath,
        }).ToList();
    }
}