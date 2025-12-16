namespace TippingRoster.Application.Interfaces;

public interface ITipCalculationService
{
    IReadOnlyDictionary<Guid, decimal> CalculateTipSplit(IReadOnlyDictionary<Guid, double> hoursWorkedPerEmployee, decimal totalTips);
}