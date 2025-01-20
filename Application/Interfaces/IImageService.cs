using Application.DTO.Image;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IImageService
{
    Task UploadImageAsync(Guid userId, IFormFile imageFile);
    Task<IEnumerable<string>> GetUserImagesAsync(Guid userId);
}