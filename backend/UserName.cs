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
}
