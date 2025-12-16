using TippingRoster.Domain.Entities;

namespace TippingRoster.Application.Interfaces;

public interface IRosterService
{
    IReadOnlyDictionary<Guid, double> GetWeeklyHoursPerEmployee(DateOnly weekStartDate);
    IReadOnlyList<Shift> GetWeeklyShifts(DateOnly weekStartDate);
}