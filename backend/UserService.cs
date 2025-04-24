using Supabase;
using Supabase.Postgrest;
using System.Collections.Generic;

namespace TodoApi;

public class UserService
{
    private readonly Supabase.Client _supabase;

    public UserService(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    // ユーザー重複チェックのドメインサービス
    public async Task<bool> Exists(User user)
    {
        try
        {
            var response = await _supabase
                .From<UserModel>()
                .Select("id")
                .Match(new Dictionary<string, string> { { "id", user.Id.ToString() } })
                .Get();

            // レスポンスのデータが存在するかどうかで重複を判定
            return response.Models.Any();
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

