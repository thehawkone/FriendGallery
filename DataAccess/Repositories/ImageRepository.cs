using System.Net.Mime;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;

    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddImageAsync(Image image)
    {
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Image>> GetImagesByUserIdAsync(Guid userId)
    {
        return await _context.Images
            .Where(i => i.UserId == userId)
            .ToListAsync();
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