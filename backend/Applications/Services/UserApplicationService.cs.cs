﻿namespace TodoApi;

public class UserApplicationService
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;

    public UserApplicationService(IUserRepository userRepository, UserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task<UserData?> Register(string name)
    {
        var user = User.CreateUser(name);

        if (await _userService.Exists(user))
        {
            throw new Exception("ユーザーが既に存在します。");
        }

        await _userRepository.Save(user);

        var userData = new UserData(user);
        return userData;
    }

    public async Task<UserData?> Get(string id)
    {
        var targetId = new UserId(id);
        var user = await _userRepository.Find(targetId);
        if (user == null)
        {
            // throw new Exception("ユーザーが見つかりません。");
            return null;
        }

        // return user;
        // ドメインオブジェクト（user）をそのまま返すと、外部から本来意図しない操作やビジネスロジックの乱用が可能になる為、
        // DTO(userData)に変換して返す。
        // 例: user.ChangeName("なまえかわる");など User のメソッドを呼べてしまう。
        // また、Domain の呼び出しは アプリケーションサービスから一貫して行う事で、保守性が高まる。
        var userData = new UserData(user);
        return userData;
    }

    public async Task<UserData?> Update(string id, string name = null, string email = null)
    {
        var targetId = new UserId(id);
        var user = await _userRepository.Find(targetId);
        if (user == null)
        {
            // throw new Exception("ユーザーが見つかりません。");
            return null;
        }
        var newUser = User.CreateUser(name);

        // 名前の更新
        if (name != null)
        {
            if (await _userService.Exists(newUser))
            {
                throw new Exception("ユーザーが既に存在します。");
            }
            user.ChangeName(name);
        }

        // メールアドレスの更新 (今後もUpdateの引数が増えるたびに、内部の処理(シグネチャー)を変更する必要が出てきてしまう。)
        if (email != null)
        {
            user.ChangeMailAddress(email);
        }

        await _userRepository.Save(user);

        var userData = new UserData(user);
        return userData;
    }
}