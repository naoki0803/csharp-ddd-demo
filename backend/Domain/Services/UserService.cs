namespace TodoApi;

using System.Threading.Tasks;
using TodoApi.Infrastructure.Repository;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // ユーザー重複チェックのドメインサービス
    public async Task<bool> Exists(UserName userName)
    {
        try
        {
            var found = await _userRepository.Find(userName);
            return found != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"重複チェック時にエラーが発生: {ex.Message}");
            // エラーの場合は重複していないとみなす
            return false;
        }
    }

    // ドメインサービスにエンティティのすべてのロジックを記述することは技術的には可能ですが、
    // それはアンチパターンです。なぜなら：
    // 1. ドメイン貧血症（Domain Anemia）を引き起こし、エンティティが単なるデータ構造になってしまう
    // 2. ビジネスロジックの意図や関連性が不明確になり、コードの保守性が低下する
    // 3. オブジェクト指向の本質的な利点である、データとロジックのカプセル化が失われる
    // public static User CreateUser(string id, string name)
    // {
    //     var user = new User();
    //     // user.Id = name; // 値オブジェクトを採用していない場合、引数のnameをプロパティに設定できてしまう。
    //     user.Id = new UserId(id); // 値オブジェクトを採用している場合、引数のnameをUserid型に変換して設定する。
    //     user.Name = new UserName(name);

    //     return user;
    // }

}

