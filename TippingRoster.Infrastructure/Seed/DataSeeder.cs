using TippingRoster.Domain.Entities;
using TippingRoster.Infrastructure.Data;

namespace TippingRoster.Infrastructure.Seed;

public static class DataSeeder
{
    public static void Seed(InMemoryDataContext context)
    {
        if (context.Employees.Any())
            return;

        var alice = new Employee(Guid.NewGuid(), "Alice");
        var bob = new Employee(Guid.NewGuid(), "Bob");

        context.Employees.AddRange(new[] { alice, bob });

        var today = DateTime.Today;
        var weekStartDateTime = today.AddDays(1);
        var weekStart = DateOnly.FromDateTime(weekStartDateTime);

        context.Shifts.AddRange(new[]
        {
            new Shift(
                Guid.NewGuid(),
                alice.Id,
                weekStart,
                weekStartDateTime.AddHours(9),
                weekStartDateTime.AddHours(17)
            ),
            new Shift(
                Guid.NewGuid(),
                alice.Id,
                weekStart.AddDays(2),
                weekStartDateTime.AddDays(2).AddHours(9),
                weekStartDateTime.AddDays(2).AddHours(17)
            ),
            new Shift(
                Guid.NewGuid(),
                bob.Id,
                weekStart.AddDays(1),
                weekStartDateTime.AddDays(1).AddHours(10),
                weekStartDateTime.AddDays(1).AddHours(15)
            )
        });

        context.WeeklyTips = new WeeklyTips(weekStart, 300);
    }
}
