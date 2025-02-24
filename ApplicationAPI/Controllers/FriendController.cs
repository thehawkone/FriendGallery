﻿using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FriendController : ControllerBase
{
    private readonly IFriendService _friendService;

    public FriendController(IFriendService friendService)
    {
        _friendService = friendService;
    }

    [HttpPost("add-friend")]
    public async Task<IActionResult> AddFriend([FromQuery] Guid friendId)
    {
        await _friendService.AddFriend(friendId);
        return Ok("Заявка в друзья отправлена");
    }

    [HttpPut("confirm-friend")]
    public async Task<IActionResult> ConfirmFriend([FromQuery] Guid friendId)
    {
        await _friendService.ConfirmFriendshipAsync(friendId);
        return Ok("Заявка в друзья успешно принята");
    }

    [HttpGet("get-friendships")]
    public async Task<IActionResult> GetFriendships([FromQuery] Guid userId)
    {
        var friendships = await _friendService.GetFriendshipsAsync(userId);
        return Ok(friendships);
    }

    [HttpGet("get-friend-images")]
    public async Task<IActionResult> GetFriendImages([FromQuery] Guid friendId)
    {
        var images = await _friendService.GetFriendImagesAsync(friendId);
        return Ok(images);
    }
}