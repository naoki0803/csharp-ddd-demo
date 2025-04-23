namespace TodoApi;

public class UserName : IEquatable<UserName>
{
    private readonly string value;

    public UserName(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (value.Length < 3) throw new ArgumentException("ユーザー名は3文字以上です。");
        this.value = value;
    }

    public override string ToString()
    {
        return value;
    }

    public bool Equals(UserName other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(value, other.value);
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
            return value != null ? value.GetHashCode() : 0;
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
