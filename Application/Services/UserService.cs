using Application.DTO.User;
using DataAccess;
using Domain.Abstractions;
using Domain.Models;

namespace Application.Services;

public class UserService
{
    private readonly TokenService _tokenService;
    private readonly AppDbContext _appDbContext;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, AppDbContext appDbContext, TokenService tokenService)
    {
        _userRepository = userRepository;
        _appDbContext = appDbContext;
        _tokenService = tokenService;
    }

    public async Task<UserDto> RegisterAsync(UserLoginRegisterDto userLoginRegisterDto)
    {
        var passwordHash = HashPassword(userLoginRegisterDto.Password);

        var user = new User
        {
            UserId = Guid.NewGuid(),
            PasswordHash = passwordHash,
            Username = userLoginRegisterDto.Name
        };
        
        await _userRepository.CreateUserAsync(user);
        await _appDbContext.SaveChangesAsync();
        
        return new UserDto { UserId = Guid.NewGuid(), Username = user.Username };
    }

    public async Task<string> LoginAsync(string name, string password)
    {
        var user = await _userRepository.GetUserByName(name);
        if (!VerifyPassword(password, user.PasswordHash)) {
            throw new Exception("Пароль или логин неверный!");
        }

        return _tokenService.GenerateToken(user.UserId);
    }

    public async Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) {
            throw new Exception("Пользователь не найден");
        }

        if (!VerifyPassword(oldPassword, user.PasswordHash)) {
            throw new Exception("Неверный пароль!");
        }

        if (oldPassword == newPassword) {
            throw new Exception("Новый пароль не должен совпадать со старым");
        }

        var newHashedPassword = HashPassword(newPassword);
        user.PasswordHash = newHashedPassword;
        
        await _userRepository.UpdateUserAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) {
            throw new Exception("Пользователь не найден");
        }
        
        await _userRepository.DeleteUserAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}