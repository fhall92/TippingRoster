using Microsoft.AspNetCore.Mvc;
using TippingRoster.Application.Interfaces;
using TippingRoster.Application.Queries;

namespace TippingRoster.Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] GetEmployeesQuery query)
    {
        return Ok(_employeeRepository.GetAll());
    }
}
