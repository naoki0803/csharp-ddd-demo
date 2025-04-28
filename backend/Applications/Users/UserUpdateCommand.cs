namespace TodoApi;

public class UserUpdateCommand
{
    public UserUpdateCommand(string id)
    {
        Id = id;
    }
    public string Id { get; }
    public string? Name { get; set; }
    public string? Email { get; set; }

    // 以下のように、コンストラクタの引数で id 以外を null で表現してもよい
    // public UserUpdateCommand(string id, string name = null, string email = null)
    // {
    //     Id = id;
    //     Name = name;
    //     Email = email;
    // }

    // public string Id { get; }
    // public string Name { get; }
    // public string Email { get; }

}
