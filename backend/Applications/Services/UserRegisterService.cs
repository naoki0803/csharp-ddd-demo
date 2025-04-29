namespace TodoApi;

public class UserRegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;

    public UserRegisterService(IUserRepository userRepository, UserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task<UserData?> Handle(UserRegisterCommand command)
    {
        var user = User.CreateUser(command.Name);

        if (await _userService.Exists(user.Name))
        {
            throw new Exception("ユーザーが既に存在します。");
        }

        await _userRepository.Save(user);

        var userData = new UserData(user);
        return userData;
    }
}
