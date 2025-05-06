namespace TodoApi;

public class UserPostRequestModel
{
    public UserPostRequestModel(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
}
