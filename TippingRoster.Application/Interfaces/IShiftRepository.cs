using TippingRoster.Domain.Entities;

namespace TippingRoster.Application.Interfaces;

public interface IShiftRepository
{
    IReadOnlyList<Shift> GetShiftsForWeek(DateOnly weekStartDate);

    Shift? GetById(Guid id);

    void Add(Shift shift);

    void Update(Shift shift);

    void Delete(Guid id);
}
