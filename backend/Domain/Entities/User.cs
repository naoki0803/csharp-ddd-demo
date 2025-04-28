namespace TodoApi;

public class User : IEquatable<User>
{
    public UserId Id { get; private set; }
    public UserName Name { get; private set; }

    // コンストラクタをprivateにして、インスタンスを生成することを禁止する。
    // ファクトリ関数で CreateUser を呼び出す。
    private User() { }

    // 値オブジェクトではなく、プリミティブな値をIdに採用した場合、
    // CreateUser メソッド内で user.Id = name; としても、コンパイルエラーが発生しない
    // public string Id { get; set; }
    public static User CreateUser(string name)
    {
        var user = new User();
        // user.Id = name; // 値オブジェクトを採用していない場合、引数のnameをプロパティに設定できてしまう。
        user.Id = new UserId(Guid.NewGuid().ToString()); // 値オブジェクトを採用している場合、引数のnameをUserid型に変換して設定する。
        // user.Id = new UserId("c0d3fd05-1bea-4d69-8689-ac5a4209f7b2"); // 重複チェックの為一時的にハードコーディング
        user.Name = new UserName(name);

        return user;
    }

    // DBから取得したIDと名前を使ってUserインスタンスを「復元」するメソッド
    public static User Reconstruct(string id, string name)
    {
        var user = new User();
        user.Id = new UserId(id);
        user.Name = new UserName(name);
        return user;
    }

    public void ChangeName(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (name.Length < 3) throw new ArgumentException("ユーザー名は3文字以上です。", nameof(name));
        Name = new UserName(name);
    }

    public void ChangeMailAddress(string mailAddress)
    {
        // メールアドレスの更新 (未実装)
    }

    // User クラスに重複確認の定義することは不自然な為コメントアウト ※UserService.csで実装指定いる
    // Userエンティティは自分自身の情報（ID、名前、削除状態など）は知っているが、他のUserの存在は知らない為、重複確認はUserエンティティの責務ではない。
    // public bool Exists(User user)
    // {
    //     return true;
    //     // 重複確認のコード
    // }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}";
    }


    public bool Equals(User other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return Id != null ? Id.GetHashCode() : 0;
        }
    }

    public static bool operator ==(User? left, User? right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(User? left, User? right)
    {
        return !(left == right);
    }
}
