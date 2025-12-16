using TippingRoster.Domain.Entities;

namespace TippingRoster.Infrastructure.Data;

public class InMemoryDataContext
{
    public List<Employee> Employees { get; } = new();
    public List<Shift> Shifts { get; } = new();
    public WeeklyTips WeeklyTips { get; set; } = null!;
}
