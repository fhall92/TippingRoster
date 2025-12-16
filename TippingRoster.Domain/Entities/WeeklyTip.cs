namespace TippingRoster.Domain.Entities;

public class WeeklyTips
{
    public DateOnly WeekStartDate { get; }
    public decimal TotalAmount { get; }

    public WeeklyTips(DateOnly weekStartDate, decimal totalAmount)
    {
        if (totalAmount < 0)
            throw new ArgumentException("Total tips cannot be negative");

        WeekStartDate = weekStartDate;
        TotalAmount = totalAmount;
    }
}
