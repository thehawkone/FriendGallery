﻿using System.Security.Claims;
using Application.DTO.User;
using DataAccess;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class FriendService
{
    private readonly TokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public FriendService(IUserRepository userRepository, TokenService tokenService, AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddFriend(Guid friendId)
    {
        var userId = GetCurrentUserId();
        var friend = _userRepository.GetUserByIdAsync(friendId);
        if (friend == null || userId == null) {
            throw new Exception("Пользователь или друг не найден");
        }
        
        var existingFriend = await _appDbContext.UserFriends
            .FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == friendId);
        if (existingFriend != null) {
            throw new Exception("Вы уже являетесь друзьями!");
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