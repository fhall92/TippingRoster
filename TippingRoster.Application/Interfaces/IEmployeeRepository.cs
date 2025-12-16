using TippingRoster.Domain.Entities;

namespace TippingRoster.Application.Interfaces;

public interface IEmployeeRepository
{
    IReadOnlyList<Employee> GetAll();
    Employee? GetById(Guid id);
}
