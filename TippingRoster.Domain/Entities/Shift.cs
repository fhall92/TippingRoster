namespace TippingRoster.Domain.Entities;

public class Shift
{
    public Guid Id { get; }
    public Guid EmployeeId { get; }
    public DateOnly Date { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    public double TotalHours { get; }

    public Shift(Guid id,Guid employeeId, DateOnly date, DateTime startTime, DateTime endTime)
    {
        if(endTime <= startTime)
        {
            throw new ArgumentException("End time must be after start time");
        }

        Id = id;
        EmployeeId = employeeId;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
        TotalHours = GetTotalHours();
    }

    private double GetTotalHours() => (EndTime - StartTime).TotalHours;
}
