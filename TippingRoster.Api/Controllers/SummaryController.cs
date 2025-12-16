using Microsoft.AspNetCore.Mvc;
using TippingRoster.Application.Interfaces;
using TippingRoster.Application.Services;
using TippingRoster.Application.Queries;

namespace TippingRoster.Api.Controllers;

[ApiController]
[Route("api/summary")]
public class SummaryController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITipsRepository _tipsRepository;
    private readonly RosterService _rosterService;
    private readonly TipCalculationService _tipCalculationService;

    public SummaryController(
        IEmployeeRepository employeeRepository,
        ITipsRepository tipsRepository,
        RosterService rosterService,
        TipCalculationService tipCalculationService)
    {
        _employeeRepository = employeeRepository;
        _tipsRepository = tipsRepository;
        _rosterService = rosterService;
        _tipCalculationService = tipCalculationService;
    }

    [HttpGet("week")]
    public IActionResult GetWeeklySummary([FromQuery] GetWeeklySummaryQuery query)
    {
        var weekStart = query?.WeekStart ?? GetCurrentWeekStart();

        var employees = _employeeRepository.GetAll();
        var weeklyTips = _tipsRepository.GetCurrentWeek();

        var hoursPerEmployee = _rosterService.GetWeeklyHoursPerEmployee(weekStart);
        var tipSplit = _tipCalculationService.CalculateTipSplit(
            hoursPerEmployee,
            weeklyTips.TotalAmount
        );

        var result = employees.Select(e => new
        {
            employeeId = e.Id,
            name = e.Name,
            hoursWorked = hoursPerEmployee.GetValueOrDefault(e.Id, 0),
            tipAmount = tipSplit.GetValueOrDefault(e.Id, 0)
        });

        return Ok(new
        {
            totalTips = weeklyTips.TotalAmount,
            employees = result
        });
    }

    private static DateOnly GetCurrentWeekStart()
    {
        var today = DateTime.Today;
        var monday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
        return DateOnly.FromDateTime(monday);
    }
}
