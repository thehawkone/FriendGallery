namespace Domain.Models;

public class Image
{
    public Guid PhotoId { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}