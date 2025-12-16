namespace TippingRoster.Domain.Entities;

public class Employee
{
    public Guid Id { get; }
    public string Name { get; }

    public Employee(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
