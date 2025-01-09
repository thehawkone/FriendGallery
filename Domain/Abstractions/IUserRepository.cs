using Domain.Models;

namespace Domain.Abstractions;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> GetUserByName(string name);
    Task<User> GetUserWithImagesAsync(Guid userId);
    Task<User> GetUserWithFriendsAsync(Guid userId);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
}