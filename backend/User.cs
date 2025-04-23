namespace TodoApi;

public class User
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

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}";
    }
}
