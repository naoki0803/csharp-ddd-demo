namespace TodoApi;

public class UserName
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

    public override bool Equals(object obj)
    {
        if (obj is UserName other)
        {
            return this.value == other.value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
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
