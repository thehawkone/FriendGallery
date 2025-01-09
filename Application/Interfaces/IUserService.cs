using Application.DTO.User;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserLoginRegisterDto userLoginRegisterDto);
    Task<string> LoginAsync(string name, string password);
    Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    Task DeleteAsync(Guid userId);
}