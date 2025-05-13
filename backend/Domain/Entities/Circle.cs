namespace TodoApi;

public class Circle : IEquatable<Circle>
{
    public CircleId Id { get; private set; } = null!;
    public CircleName Name { get; private set; } = null!;
    public User Owner { get; private set; } = null!;
    public List<User> Members { get; private set; } = null!;

    private Circle(CircleId id, CircleName name, User owner, List<User> members)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (owner == null) throw new ArgumentNullException(nameof(owner));
        if (members == null) throw new ArgumentNullException(nameof(members));

        Id = id;
        Name = name;
        Owner = owner;
        Members = members;
    }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}";
    }


    public bool Equals(Circle? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Circle)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return Id.GetHashCode();
        }
    }

    public static bool operator ==(Circle? left, Circle? right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(Circle? left, Circle? right)
    {
        return !(left == right);
    }
}
