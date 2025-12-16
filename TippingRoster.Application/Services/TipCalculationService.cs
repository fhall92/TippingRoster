namespace TippingRoster.Application.Services;

public class TipCalculationService
{
    public IReadOnlyDictionary<Guid, decimal> CalculateTipSplit(IReadOnlyDictionary<Guid, double> hoursWorkedPerEmployee, decimal totalTips)
    {
        if (totalTips <= 0 || hoursWorkedPerEmployee.Count == 0)
            return new Dictionary<Guid, decimal>();

        var totalHours = hoursWorkedPerEmployee.Values.Sum();

        if (totalHours <= 0)
            return new Dictionary<Guid, decimal>();

        return hoursWorkedPerEmployee.ToDictionary(
            kvp => kvp.Key,
            kvp => decimal.Round((decimal)(kvp.Value / totalHours) * totalTips, 2)
        );
    }
}
