using TippingRoster.Application.Interfaces;
using TippingRoster.Domain.Entities;
using TippingRoster.Infrastructure.Data;

namespace TippingRoster.Infrastructure.Repositories;

public class ShiftRepository : IShiftRepository
{
    private readonly InMemoryDataContext _context;

    public ShiftRepository(InMemoryDataContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Shift> GetShiftsForWeek(DateOnly weekStartDate)
    {
        var weekEnd = weekStartDate.AddDays(7);

        return _context.Shifts
            .Where(s => s.Date >= weekStartDate && s.Date < weekEnd)
            .ToList();
    }

    public Shift? GetById(Guid id)
    {
        return _context.Shifts.FirstOrDefault(s => s.Id == id);
    }

    public void Add(Shift shift)
    {
        _context.Shifts.Add(shift);
    }

    public void Update(Shift shift)
    {
        var index = _context.Shifts.FindIndex(s => s.Id == shift.Id);
        if (index >= 0)
        {
            _context.Shifts[index] = shift;
        }
    }

    public void Delete(Guid id)
    {
        var shift = GetById(id);
        if (shift != null)
        {
            _context.Shifts.Remove(shift);
        }
    }
}
