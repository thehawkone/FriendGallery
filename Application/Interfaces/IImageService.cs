using Application.DTO.Image;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IImageService
{
    Task UploadImageAsync(Guid userId, IFormFile imageFile);
}