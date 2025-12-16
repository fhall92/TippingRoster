namespace TippingRoster.Application.Commands;

public class CreateShiftCommand
{
    public Guid EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}