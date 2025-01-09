namespace Application.DTO.User;

public class ChangePasswordDto : UserLoginRegisterDto
{
    public string NewPassword { get; set; }
}