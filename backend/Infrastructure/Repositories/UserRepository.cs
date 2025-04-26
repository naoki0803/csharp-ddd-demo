namespace TodoApi.Infrastructure.Repository;

using TodoApi.Infrastructure.Models;


public class UserRepository : IUserRepository
{
    private readonly Supabase.Client _supabase;

    public UserRepository(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    public async Task Save(User user)
    {
        try
        {
            var model = new UserModel
            {
                Id = user.Id.ToString(),
                Name = user.Name.ToString()
            };

            var result = await _supabase.From<UserModel>().Insert(model);
            Console.WriteLine($"{user}の保存に成功しました。");
        }
        catch (Exception ex)
        {
            // ログ出力
            Console.WriteLine($"ユーザー保存中にエラーが発生: {ex.Message}");
            // カスタム例外に変換して再スロー
            throw new Exception($"ユーザー {user} の保存に失敗しました", ex);
        }
    }

    public async Task<User> Find(UserName name)
    {
        try
        {
            var response = await _supabase.From<UserModel>()
                .Select("*")
                .Match(new Dictionary<string, string> { { "name", name.ToString() } })
                .Get();
            Console.WriteLine($"検索結果: {response?.Models?.Count ?? 0} 件");

            var result = response?.Models?.FirstOrDefault();
            if (result == null) return null;
            return User.CreateUser(result.Name);
        }
        catch (Exception ex)
        {
            // ログ出力
            Console.WriteLine($"ユーザー検索中にエラーが発生: {ex.Message}");
            // カスタム例外に変換して再スロー
            throw new Exception($"ユーザー {name} の検索に失敗しました", ex);
        }
    }
}


