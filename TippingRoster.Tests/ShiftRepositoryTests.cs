using System;
using System.Linq;
using Xunit;
using TippingRoster.Infrastructure.Data;
using TippingRoster.Infrastructure.Repositories;
using TippingRoster.Domain.Entities;

namespace TippingRoster.Tests.Repositories
{
    public class ShiftRepositoryTests
    {
        [Fact]
        public void Add_And_GetById_Works()
        {
            var ctx = new InMemoryDataContext();
            var repo = new ShiftRepository(ctx);

            var shift = new Shift(Guid.NewGuid(), Guid.NewGuid(), new DateOnly(2025, 12, 15), new DateTime(2025,12,15,9,0,0), new DateTime(2025,12,15,10,0,0));
            repo.Add(shift);

            var fetched = repo.GetById(shift.Id);

            Assert.NotNull(fetched);
            Assert.Equal(shift.Id, fetched!.Id);
        }

        [Fact]
        public void GetShiftsForWeek_FiltersByWeekRange()
        {
            var ctx = new InMemoryDataContext();
            var repo = new ShiftRepository(ctx);

            var weekStart = new DateOnly(2025, 12, 15);
            var inWeek = new Shift(Guid.NewGuid(), Guid.NewGuid(), weekStart, new DateTime(2025,12,15,9,0,0), new DateTime(2025,12,15,10,0,0));
            var outWeek = new Shift(Guid.NewGuid(), Guid.NewGuid(), weekStart.AddDays(7), new DateTime(2025,12,22,9,0,0), new DateTime(2025,12,22,10,0,0));

            repo.Add(inWeek);
            repo.Add(outWeek);

            var result = repo.GetShiftsForWeek(weekStart);

            Assert.Single(result);
            Assert.Contains(inWeek, result);
            Assert.DoesNotContain(outWeek, result);
        }

        [Fact]
        public void Update_ReplacesExistingShift()
        {
            var ctx = new InMemoryDataContext();
            var repo = new ShiftRepository(ctx);

            var id = Guid.NewGuid();
            var original = new Shift(id, Guid.NewGuid(), new DateOnly(2025, 12, 15), new DateTime(2025,12,15,9,0,0), new DateTime(2025,12,15,10,0,0));
            repo.Add(original);

            var updated = new Shift(id, original.EmployeeId, original.Date, new DateTime(2025,12,15,11,0,0), new DateTime(2025,12,15,14,0,0));
            repo.Update(updated);

            var fetched = repo.GetById(id);
            Assert.NotNull(fetched);
            Assert.Equal(updated.StartTime, fetched!.StartTime);
            Assert.Equal(updated.EndTime, fetched.EndTime);
        }

        [Fact]
        public void Delete_RemovesShift()
        {
            var ctx = new InMemoryDataContext();
            var repo = new ShiftRepository(ctx);

            var shift = new Shift(Guid.NewGuid(), Guid.NewGuid(), new DateOnly(2025, 12, 15), new DateTime(2025,12,15,9,0,0), new DateTime(2025,12,15,10,0,0));
            repo.Add(shift);

            repo.Delete(shift.Id);

            var fetched = repo.GetById(shift.Id);
            Assert.Null(fetched);
        }
    }
}