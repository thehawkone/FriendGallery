using Domain.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;

    public ImageRepository(AppDbContext context) {
        _context = context;
    }

    public async Task AddImageAsync(Image image)
    {
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();
    }

    public async Task<Image> GetImageAsync(Image image)
    {
        return await _context.Images.FindAsync(image.PhotoId);
    }

    public async Task UpdateImageAsync(Image image)
    {
        _context.Images.Update(image);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteImageAsync(Image image)
    {
        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
    }
}