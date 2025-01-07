namespace Domain.Models;

public class Image
{
    public Guid PhotoId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; }
}