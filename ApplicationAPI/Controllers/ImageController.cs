using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromQuery] Guid userId, string imagePath)
    {
        await _imageService.UploadImageAsync(userId, imagePath);
        return Ok("Фотография успешно загружена");
    }

    [HttpGet("user-images")]
    public async Task<IActionResult> GetUserImages([FromQuery] Guid userId)
    {
        await _imageService.GetUserImagesAsync(userId);
        return Ok("Успешно");
    }

    public async Task<IActionResult> GetFriendImages([FromQuery] Guid userId, Guid friendId)
    {
        await _imageService.GetFriendImagesAsync(userId, friendId);
        return Ok("Успешно");
    }
}