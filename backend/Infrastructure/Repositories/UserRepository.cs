namespace TodoApi.Infrastructure.Repository;

using TodoApi.Infrastructure.Models;


public class UserRepository : IUserRepository
{
    private readonly Supabase.Client _supabase;

    public UserRepository(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    public void Save(User user)
    {
        // 既存のユーザーを更新または新規ユーザーを追加
        // var existingUser = users.FirstOrDefault(u => u.Id.Equals(user.Id));
        // if (existingUser != null)
        // {
        //     users.Remove(existingUser);
        // }
        // users.Add(user);
    }

    public async Task<User> Find(UserName name)
    {
        try
        {
            var result = await _supabase.From<UserModel>()
                .Where(u => u.Name == name.ToString())
                .Single();

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


