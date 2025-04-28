namespace TodoApi;
/// <summary>
/// ユーザーIDを表すバリューオブジェクト
/// </summary>
public class UserId : IEquatable<UserId>
{
    // private readonly string value;
    public string Value { get; }

    public UserId(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    public bool Equals(UserId other)
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
        return Equals((UserId)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(UserId left, UserId right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(UserId left, UserId right)
    {
        return !(left == right);
    }
}
