﻿namespace TodoApi;
/// <summary>
/// ユーザーIDを表すバリューオブジェクト
/// </summary>
public class UserId
{
    private readonly string value;

    public UserId(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        this.value = value;
    }

    public override string ToString()
    {
        return value;
    }
}
