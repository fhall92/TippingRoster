using TippingRoster.Application.Interfaces;
using TippingRoster.Domain.Entities;
using TippingRoster.Infrastructure.Data;

namespace TippingRoster.Infrastructure.Repositories;

public class TipsRepository : ITipsRepository
{
    private readonly InMemoryDataContext _context;

    public TipsRepository(InMemoryDataContext context)
    {
        _context = context;
    }

    public WeeklyTips GetCurrentWeek()
    {
        return _context.WeeklyTips;
    }
}
