namespace TodoApi;

public interface IUserRepository
{
    void Save(User user);
    Task<User> Find(UserName name);
}