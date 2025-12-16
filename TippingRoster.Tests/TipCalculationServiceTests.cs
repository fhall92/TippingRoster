using TippingRoster.Application.Services; 
using Xunit;

namespace TippingRoster.Tests
{
    public class TipCalculationServiceTests
    {
        [Fact]
        public void CalculateTipSplit_ReturnsEmpty_WhenTotalTipsIsZero()
        {
            var svc = new TipCalculationService();

            var hours = new Dictionary<Guid, double>
            {
                [Guid.NewGuid()] = 8.0
            };

            var result = svc.CalculateTipSplit(hours, 0m);

            Assert.Empty(result);
        }

        [Fact]
        public void CalculateTipSplit_ReturnsEmpty_WhenNoEmployees()
        {
            var svc = new TipCalculationService();

            var result = svc.CalculateTipSplit(new Dictionary<Guid, double>(), 100m);

            Assert.Empty(result);
        }

        [Fact]
        public void CalculateTipSplit_ReturnsEmpty_WhenTotalHoursIsZero()
        {
            var svc = new TipCalculationService();

            var hours = new Dictionary<Guid, double>
            {
                [Guid.NewGuid()] = 0.0,
                [Guid.NewGuid()] = 0.0
            };

            var result = svc.CalculateTipSplit(hours, 100m);

            Assert.Empty(result);
        }

        [Fact]
        public void CalculateTipSplit_DistributesProportionally_AndRoundsToTwoDecimals()
        {
            var svc = new TipCalculationService();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var hours = new Dictionary<Guid, double>
            {
                [id1] = 2.0, 
                [id2] = 6.0
            };

            var totalTips = 100m;

            var result = svc.CalculateTipSplit(hours, totalTips);

            Assert.Equal(2, result.Count);
            Assert.Equal(25.00m, result[id1]);
            Assert.Equal(75.00m, result[id2]);
        }
    }
}