using TippingRoster.Application.Interfaces;
using TippingRoster.Domain.Entities;

namespace TippingRoster.Application.Services;

public class RosterService
{
    private readonly IShiftRepository _shiftRepository;

    public RosterService(IShiftRepository shiftRepository)
    {
        _shiftRepository = shiftRepository;
    }

    public IReadOnlyDictionary<Guid, double> GetWeeklyHoursPerEmployee(DateOnly weekStartDate)
    {
        var shifts = _shiftRepository.GetShiftsForWeek(weekStartDate);

        return shifts
            .GroupBy(s => s.EmployeeId)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(s => s.TotalHours)
            );
    }

    public IReadOnlyList<Shift> GetWeeklyShifts(DateOnly weekStartDate)
    {
        return _shiftRepository.GetShiftsForWeek(weekStartDate);
    }
}
