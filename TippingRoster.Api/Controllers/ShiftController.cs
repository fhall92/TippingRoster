using Microsoft.AspNetCore.Mvc;
using TippingRoster.Application.Interfaces;
using TippingRoster.Domain.Entities;
using TippingRoster.Application.Commands;

namespace TippingRoster.Api.Controllers;

[ApiController]
[Route("api/shifts")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftRepository _shiftRepository;

    public ShiftsController(IShiftRepository shiftRepository)
    {
        _shiftRepository = shiftRepository;
    }

    [HttpGet("week")]
    public IActionResult GetCurrentWeek()
    {
        var weekStart = GetCurrentWeekStart();
        return Ok(_shiftRepository.GetShiftsForWeek(weekStart));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateShiftCommand command)
    {
        var shift = new Shift(
            Guid.NewGuid(),
            command.EmployeeId,
            command.Date,
            command.StartTime,
            command.EndTime
        );
        _shiftRepository.Add(shift);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] UpdateShiftCommand command)
    {
        var existing = _shiftRepository.GetById(id);
        if (existing == null)
            return NotFound();

        var updatedShift = new Shift(
            id,
            command.EmployeeId,
            command.Date,
            command.StartTime,
            command.EndTime
        );

        _shiftRepository.Update(updatedShift);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _shiftRepository.Delete(id);
        return Ok();
    }

    private static DateOnly GetCurrentWeekStart()
    {
        var today = DateTime.Today;
        var monday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
        return DateOnly.FromDateTime(monday);
    }
}
