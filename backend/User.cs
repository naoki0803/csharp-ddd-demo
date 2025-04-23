namespace TodoApi;

public class User : IEquatable<User>
{
    public UserId Id { get; set; }
    public UserName Name { get; set; }

    // 値オブジェクトではなく、プリミティブな値をIdに採用した場合、
    // CreateUser メソッド内で user.Id = name; としても、コンパイルエラーが発生しない
    // public string Id { get; set; }

    public static User CreateUser(string id, string name)
    {
        var user = new User();
        // user.Id = name; // 値オブジェクトを採用していない場合、引数のnameをプロパティに設定できてしまう。
        user.Id = new UserId(id); // 値オブジェクトを採用している場合、引数のnameをUserid型に変換して設定する。
        user.Name = new UserName(name);

        return user;
    }

    public void ChangeName(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (name.Length < 3) throw new ArgumentException("ユーザー名は3文字以上です。", nameof(name));
        Name = new UserName(name);
    }

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

    public static bool operator ==(User left, User right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(User left, User right)
    {
        return !(left == right);
    }
}
