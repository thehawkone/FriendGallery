namespace Domain.Models;

public class UserFriend
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public Guid FriendId { get; set; }
    public User Friend { get; set; }
}