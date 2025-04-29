namespace TodoApi;

public class UserDeleteService
{
    private readonly IUserRepository _userRepository;

    public UserDeleteService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserData?> Handle(UserDeleteCommand command)
    {
        var targetId = new UserId(command.Id);
        var user = await _userRepository.Find(targetId);
        if (user == null)
        {
            throw new Exception("ユーザーが見つかりません。");
        }

        await _userRepository.Delete(user);

        return new UserData(user);
    }
}