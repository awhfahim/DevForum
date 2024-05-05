namespace StackOverflow.Domain.Entities;

public class Tag : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    private Tag() { }
    public Tag(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}