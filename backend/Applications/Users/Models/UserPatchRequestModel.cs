namespace TodoApi;

public class UserPatchRequestModel
{
    public UserPatchRequestModel(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
