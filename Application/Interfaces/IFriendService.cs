namespace Application.Services;

public interface IFriendService
{
    Task AddFriend(Guid friendId);
    Task ConfirmFriendshipAsync(Guid friendId);
}