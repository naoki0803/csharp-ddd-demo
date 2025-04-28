namespace TodoApi;

public interface IUserRepository
{
    Task<User?> Find(UserName name);
    Task<User?> Find(UserId id);
    Task Save(User user);
    // Task Delete(User user);
}