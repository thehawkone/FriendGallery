using Domain.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User> GetUserByName(string name)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == name);
    }

    public async Task<User> GetUserWithImagesAsync(Guid userId)
    {
        return (await _context.Users
            .Include(u => u.Images)
            .FirstOrDefaultAsync(u => u.UserId == userId))!;
    }

    public async Task<User> GetUserWithFriendsAsync(Guid userId)
    {
        return (await _context.Users
            .Include(u => u.Friends)
            .ThenInclude(uf => uf.Friend)
            .FirstOrDefaultAsync(u => u.UserId == userId))!;
    }
    
    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}