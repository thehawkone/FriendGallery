namespace Application.DTO;

public class ChangePasswordDto : UserLoginRegisterDto
{
    public string NewPassword { get; set; }
}