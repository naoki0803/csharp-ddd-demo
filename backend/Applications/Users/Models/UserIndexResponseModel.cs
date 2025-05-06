namespace TodoApi;

public class UserIndexResponseModel
{
    public UserIndexResponseModel(string id, string name)
    {
        Id = id;
        Name = name;
    }
    public string Id { get; }
    public string Name { get; }
}