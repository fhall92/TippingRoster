using Xunit;
using Moq;
using TippingRoster.Application.Services;
using TippingRoster.Application.Interfaces;
using TippingRoster.Domain.Entities;

namespace TippingRoster.Tests
{
    public class RosterServiceTests
    {
        [Fact]
        public void GetWeeklyHoursPerEmployee_AggregatesTotalHoursPerEmployee_UsingMoq()
        {
            // Arrange
            var weekStart = new DateOnly(2025, 12, 15); // Monday
            var e1 = Guid.NewGuid();
            var e2 = Guid.NewGuid();

            var shifts = new[]
            {
                // e1: 3h + 2h = 5h
                new Shift(Guid.NewGuid(), e1, weekStart, new DateTime(2025,12,15,9,0,0), new DateTime(2025,12,15,12,0,0)),
                new Shift(Guid.NewGuid(), e1, weekStart.AddDays(1), new DateTime(2025,12,16,10,0,0), new DateTime(2025,12,16,12,0,0)),
                // e2: 4h
                new Shift(Guid.NewGuid(), e2, weekStart.AddDays(2), new DateTime(2025,12,17,8,0,0), new DateTime(2025,12,17,12,0,0))
            }.ToList();

            var mockRepo = new Mock<IShiftRepository>();
            mockRepo.Setup(r => r.GetShiftsForWeek(weekStart)).Returns(shifts);

            var svc = new RosterService(mockRepo.Object);

            // Act
            var result = svc.GetWeeklyHoursPerEmployee(weekStart);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(5.0, result[e1]);
            Assert.Equal(4.0, result[e2]);

            mockRepo.Verify(r => r.GetShiftsForWeek(weekStart), Times.Once);
        }

        [Fact]
        public void GetWeeklyShifts_ReturnsOnlyShiftsForTheProvidedWeek_UsingMoq()
        {
            // Arrange
            var weekStart = new DateOnly(2025, 12, 15);
            var inWeek = new Shift(Guid.NewGuid(), Guid.NewGuid(), weekStart, new DateTime(2025,12,15,9,0,0), new DateTime(2025,12,15,11,0,0));
            var outWeek = new Shift(Guid.NewGuid(), Guid.NewGuid(), weekStart.AddDays(8), new DateTime(2025,12,23,9,0,0), new DateTime(2025,12,23,11,0,0));

            var mockRepo = new Mock<IShiftRepository>();
            mockRepo.Setup(r => r.GetShiftsForWeek(weekStart)).Returns(new List<Shift> { inWeek });

            var svc = new RosterService(mockRepo.Object);

            // Act
            var result = svc.GetWeeklyShifts(weekStart);

            // Assert
            Assert.Single(result);
            Assert.Contains(inWeek, result);
            Assert.DoesNotContain(outWeek, result);

            mockRepo.Verify(r => r.GetShiftsForWeek(weekStart), Times.Once);
        }
    }
}
