namespace TodoApi;

public class UserName : IEquatable<UserName>
{
    // private readonly string value;
    public string Value { get; }

    public UserName(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (value.Length < 3) throw new ArgumentException("ユーザー名は3文字以上です。");
        if (value.Length >= 20) throw new ArgumentException("ユーザー名は20文字以下です。");
        // if (value.Length < 6) throw new ArgumentException("ユーザー名は6文字以上です。");   // ドメインルールを3文字以上から6文字以上に変更(変更箇所がここの一箇所だけになる)
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    public bool Equals(UserName other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Value, other.Value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UserName)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return Value != null ? Value.GetHashCode() : 0;
        }
    }

    public static bool operator ==(UserName left, UserName right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(UserName left, UserName right)
    {
        return !(left == right);
    }
}
