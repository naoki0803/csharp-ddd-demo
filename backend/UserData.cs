namespace TodoApi;

// パラメーターを個別に受け取るパターン
// public class UserData(string id, string name)
// {
//     public string Id { get; } = id;
//     public string Name { get; } = name;
// }

// User 自体を受け取るパターン
public class UserData(User source)
{
    public string Id { get; } = source.Id.Value;
    public string Name { get; } = source.Name.Value;

    // 仮にパラメーターが増えた場合以下追加をする事で変換が容易になる。
    // public string Age { get; } = source.Age.Value;
}