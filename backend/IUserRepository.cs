namespace TodoApi;

public interface IUserRepository
{
    Task Save(User user);
    Task<User> Find(UserName name);
}