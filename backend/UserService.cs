namespace TodoApi;

public class UserService
{
    // ユーザー重複チェックのドメインサービス
    public bool Exists(User user)
    {
        // 重複確認のコード
        // 今は記述していないが、本来はリポジトリを参照してuserの存在を確認する。
        return user.Id != user.Id;
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

