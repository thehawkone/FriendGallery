using Application.DTO.User;
using Domain.Models;

namespace Application.Services;

public interface IFriendService
{
    Task AddFriend(Guid friendId);
    Task ConfirmFriendshipAsync(Guid friendId);
    Task<List<UserDto>> GetFriendshipsAsync(Guid userId);
    Task<bool> IsFriendAsync(Guid userId, Guid friendId);
    Task<IEnumerable<Image>> GetFriendImagesAsync(Guid friendId);
}