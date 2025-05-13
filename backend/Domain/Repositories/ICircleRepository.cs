namespace TodoApi;

public interface ICircleRepository
{
    Task Save(Circle circle);
    Task<Circle?> Find(CircleName name);
    Task<Circle?> Find(CircleId id);
}