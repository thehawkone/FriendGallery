using Application.DTO.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserLoginRegisterDto userLoginRegisterDto)
    {
        await _userService.RegisterAsync(userLoginRegisterDto);
        return Ok("Пользователь успешно зарегистрирован");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRegisterDto userLoginRegisterDto)
    {
        var token = await _userService.LoginAsync(userLoginRegisterDto.Name, userLoginRegisterDto.Password);
        return Ok($"Авторизация прошла успешно!\nВаш токен: {token}");
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromQuery] Guid userId, [FromBody] ChangePasswordDto changePasswordDto)
    {
        await _userService.ChangePasswordAsync(userId, changePasswordDto.Password, changePasswordDto.NewPassword);
        return Ok("Пароль успешно изменён");
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid userId)
    {
        await _userService.DeleteAsync(userId);
        return Ok("Пользователь успешно удалён");
    }
}