using Domain.Models;

namespace Application.DTO;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
}