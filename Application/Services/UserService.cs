using DataAccess;
using Domain.Abstractions;

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
}