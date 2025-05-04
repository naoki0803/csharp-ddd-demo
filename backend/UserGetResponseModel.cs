namespace TodoApi;

public class UserGetResponseModel
{
    public UserGetResponseModel(string id, string name)
    {
        Id = id;
        Name = name;
    }
    public string Id { get; }
    public string Name { get; }
}