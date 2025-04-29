namespace TodoApi;

public class UserGetCommand
{
    public UserGetCommand(string id)
    {
        Id = id;
    }
    public string Id { get; }
}