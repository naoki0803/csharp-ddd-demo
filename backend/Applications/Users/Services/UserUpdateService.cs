namespace TodoApi;

public class UserUpdateService
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;

    public UserUpdateService(IUserRepository userRepository, UserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task<UserData?> Handle(UserUpdateCommand command)
    {
        var targetId = new UserId(command.Id);
        var user = await _userRepository.Find(targetId);
        if (user == null)
        {
            return null;
        }
        var name = command.Name;

        if (name != null)
        {
            var newUserName = new UserName(name);
            if (await _userService.Exists(newUserName))
            {
                throw new Exception("ユーザーが既に存在します。");
            }
            user.ChangeName(name);
        }

        await _userRepository.Save(user);

        var userData = new UserData(user);
        return userData;
    }
}