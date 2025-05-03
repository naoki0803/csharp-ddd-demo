namespace TodoApi;

public class UserGetAllService
{
    private readonly IUserRepository _userRepository;

    public UserGetAllService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserData>?> Handle()
    {
        var users = await _userRepository.FindAll();
        if (users == null)
        {
            // throw new Exception("ユーザーが見つかりません。");
            return null;
        }
        var usersData = users.Select(user => new UserData(user)).ToList();
        return usersData;
    }
}