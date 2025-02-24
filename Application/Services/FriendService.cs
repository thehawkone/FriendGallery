using System.Security.Claims;
using Application.DTO.User;
using DataAccess;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class FriendService : IFriendService
{
    private readonly IUserRepository _userRepository;
    private readonly IImageRepository _imageRepository;
    private readonly AppDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public FriendService(IUserRepository userRepository, IImageRepository imageRepository, AppDbContext appDbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddFriend(Guid friendId)
    {
        var userId = GetCurrentUserId();
        if (userId == friendId) throw new Exception("Нельзя добавить себя в друзья");
        
        var friend = _userRepository.GetUserByIdAsync(friendId);
        if (friend == null)  throw new Exception("Пользователь не найден");
        
        var existingFriend = await _appDbContext.UserFriends
            .FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == friendId);
        if (existingFriend != null) {
            throw new Exception("Запрос уже отправлен или вы уже являетесь друзьями");
        }
        
        var friendShip1 = new UserFriend { UserId = userId, FriendId = friendId, IsConfirmed = false};
        var friendShip2 = new UserFriend { UserId = friendId, FriendId = userId, IsConfirmed = false };
        
        _appDbContext.UserFriends.Add(friendShip1);
        _appDbContext.UserFriends.Add(friendShip2);

        await _appDbContext.SaveChangesAsync();
    }

    public async Task ConfirmFriendshipAsync(Guid friendId)
    {
        var userId = GetCurrentUserId();
        if (userId == null) {
            throw new Exception("Пользователь не найден");
        }
        
        var friendShip1 = await _appDbContext.UserFriends
            .FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == friendId && !f.IsConfirmed);
        var friendShip2 = await _appDbContext.UserFriends
            .FirstOrDefaultAsync(f => f.UserId == friendId && f.FriendId == userId && !f.IsConfirmed);
        
        if (friendShip1 == null || friendShip2 == null) {
            throw new Exception("Не найдено ожидающих запросов на добавление в друзья");
        }
        if (friendShip1.IsConfirmed || friendShip2.IsConfirmed) {
            throw new Exception("У одного из пользователей заявка в друзья уже принята");
        }
        
        friendShip1.IsConfirmed = true;
        friendShip2.IsConfirmed = true;
        
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<UserDto>> GetFriendshipsAsync(Guid userId)
    {
        var incomingRequest = await _appDbContext.UserFriends
            .Where(f => f.FriendId == userId && !f.IsConfirmed)
            .Select(f => f.User)
            .ToListAsync();
        
        return incomingRequest.Select(user => new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
        }).ToList();
    }

    public async Task<bool> IsFriendAsync(Guid userId, Guid friendId)
    {
        return await _appDbContext.UserFriends.AnyAsync(f => 
            f.UserId == userId && f.FriendId == friendId && f.IsConfirmed);
    }

    public async Task<IEnumerable<Image>> GetFriendImagesAsync(Guid friendId)
    {
        var userId = GetCurrentUserId();
        var isFriend = await IsFriendAsync(userId, friendId);

        if (!isFriend) throw new UnauthorizedAccessException("Вы не можете просматривать изображения этого пользователя");
        
        return await _imageRepository.GetImagesByUserIdAsync(friendId);
    }
    
    private Guid GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        return Guid.Parse(userId);
    }
}