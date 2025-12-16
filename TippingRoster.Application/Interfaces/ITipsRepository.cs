using TippingRoster.Domain.Entities;

namespace TippingRoster.Application.Interfaces;

public interface ITipsRepository
{
    WeeklyTips GetCurrentWeek();
}
