using TippingRoster.Application.Interfaces;
using TippingRoster.Domain.Entities;
using TippingRoster.Infrastructure.Data;

namespace TippingRoster.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly InMemoryDataContext _context;

    public EmployeeRepository(InMemoryDataContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Employee> GetAll()
    {
        return _context.Employees;
    }

    public Employee? GetById(Guid id)
    {
        return _context.Employees.FirstOrDefault(e => e.Id == id);
    }
}
