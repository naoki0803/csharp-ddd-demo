namespace TodoApi;

public class UserGetService
{
    private readonly IUserRepository _userRepository;

    public UserGetService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserData?> Handle(UserGetCommand command)
    {
        var targetId = new UserId(command.Id);
        var user = await _userRepository.Find(targetId);
        if (user == null)
        {
            // throw new Exception("ユーザーが見つかりません。");
            return null;
        }
        var userData = new UserData(user);
        return userData;
    }
}