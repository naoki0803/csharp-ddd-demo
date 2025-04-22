namespace TodoApi;

public class User
{
    // 値オブジェクトではなく、プリミティブな値をIdに採用した場合、
    // CreateUser メソッド内で user.Id = name; としても、コンパイルエラーが発生しない

    // public string Id { get; set; }
    public UserId Id { get; set; }
    public UserName Name { get; set; }

    public User CreateUser(string name)
    {
        var user = new User();
        // user.Id = name; // 値オブジェクトを採用していない場合、引数のnameをプロパティに設定できてしまう。
        user.Id = new UserId(name); // 値オブジェクトを採用している場合、引数のnameをUserid型に変換して設定する。
        return user;
    }


}
