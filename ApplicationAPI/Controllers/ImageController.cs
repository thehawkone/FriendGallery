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
    public async Task<IActionResult> UploadImage([FromQuery] Guid userId, IFormFile imageFile)
    {
        await _imageService.UploadImageAsync(userId, imageFile);
        return Ok("Фотография успешно загружена");
    }

    [HttpGet("viewing-my-images")]
    public async Task<IActionResult> GetUserImages(Guid userId)
    {
        var images = await _imageService.GetUserImagesAsync(userId);
        return Ok(images); 
    }
}