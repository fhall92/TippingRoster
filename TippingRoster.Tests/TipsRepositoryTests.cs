using System;
using Xunit;
using TippingRoster.Infrastructure.Data;
using TippingRoster.Infrastructure.Repositories;
using TippingRoster.Domain.Entities;

namespace TippingRoster.Tests.Repositories
{
    public class TipsRepositoryTests
    {
        [Fact]
        public void GetCurrentWeek_ReturnsWeeklyTipsFromContext()
        {
            // Arrange
            var ctx = new InMemoryDataContext();
            var weekStart = new DateOnly(2025, 12, 15);
            var weeklyTips = new WeeklyTips(weekStart, 123.45m);
            ctx.WeeklyTips = weeklyTips;

            var repo = new TipsRepository(ctx);

            // Act
            var result = repo.GetCurrentWeek();

            // Assert
            Assert.Same(weeklyTips, result);
            Assert.Equal(123.45m, result.TotalAmount);
            Assert.Equal(weekStart, result.WeekStartDate);
        }
    }
}