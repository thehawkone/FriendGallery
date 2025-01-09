using System.Net.Mime;

namespace Domain.Models;

public class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    
    private List<Image> _images = new();
    public IReadOnlyCollection<Image> Images => _images.AsReadOnly();
    
    public ICollection<UserFriend> Friends { get; set; } = new List<UserFriend>();
    public ICollection<UserFriend> AddedByFriends { get; set; } = new List<UserFriend>();
    
    public void AddImage(Image image) => _images.Add(image);
}