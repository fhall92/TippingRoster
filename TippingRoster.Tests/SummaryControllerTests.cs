using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TippingRoster.Api.Controllers;
using TippingRoster.Application.Interfaces;
using TippingRoster.Application.Services;
using TippingRoster.Application.Queries;
using TippingRoster.Domain.Entities;

namespace TippingRoster.Tests.Controllers
{
    public class SummaryControllerTests
    {
        [Fact]
        public void GetWeeklySummary_ReturnsSummary_WithCalculatedTips()
        {
            // Arrange
            var weekStart = new DateOnly(2025, 12, 15);

            var e1 = new Employee(Guid.NewGuid(), "Alice");
            var e2 = new Employee(Guid.NewGuid(), "Bob");
            var employees = new List<Employee> { e1, e2 };

            var weeklyTips = new WeeklyTips(weekStart, 100m);

            var shifts = new List<Shift>
            {
                new Shift(Guid.NewGuid(), e1.Id, weekStart, new DateTime(2025,12,15,9,0,0), new DateTime(2025,12,15,14,0,0)),
                new Shift(Guid.NewGuid(), e2.Id, weekStart.AddDays(1), new DateTime(2025,12,16,8,0,0), new DateTime(2025,12,16,11,0,0))
            };

            var mockEmployeeRepo = new Mock<IEmployeeRepository>();
            mockEmployeeRepo.Setup(r => r.GetAll()).Returns(employees);

            var mockTipsRepo = new Mock<ITipsRepository>();
            mockTipsRepo.Setup(r => r.GetCurrentWeek()).Returns(weeklyTips);

            var mockShiftRepo = new Mock<IShiftRepository>();
            mockShiftRepo.Setup(r => r.GetShiftsForWeek(weekStart)).Returns(shifts);

            var rosterSvc = new RosterService(mockShiftRepo.Object);

            var tipCalcSvc = new TipCalculationService();

            var controller = new SummaryController(
                mockEmployeeRepo.Object,
                mockTipsRepo.Object,
                rosterSvc,
                tipCalcSvc
            );

            var query = new GetWeeklySummaryQuery { WeekStart = weekStart };

            // Act
            var actionResult = controller.GetWeeklySummary(query);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(actionResult);

            var json = JsonSerializer.Serialize(ok.Value);
            using var doc = JsonDocument.Parse(json);

            var root = doc.RootElement;
            Assert.Equal(100m, root.GetProperty("totalTips").GetDecimal());

            var employeesJson = root.GetProperty("employees").EnumerateArray().ToList();
            Assert.Equal(2, employeesJson.Count);

            var aliceJson = employeesJson.Single(e => e.GetProperty("employeeId").GetGuid() == e1.Id);
            Assert.Equal("Alice", aliceJson.GetProperty("name").GetString());
            Assert.Equal(5.0, aliceJson.GetProperty("hoursWorked").GetDouble());
            Assert.Equal(62.50m, aliceJson.GetProperty("tipAmount").GetDecimal());

            var bobJson = employeesJson.Single(e => e.GetProperty("employeeId").GetGuid() == e2.Id);
            Assert.Equal("Bob", bobJson.GetProperty("name").GetString());
            Assert.Equal(3.0, bobJson.GetProperty("hoursWorked").GetDouble());
            Assert.Equal(37.50m, bobJson.GetProperty("tipAmount").GetDecimal());

            mockEmployeeRepo.Verify(r => r.GetAll(), Times.Once);
            mockTipsRepo.Verify(r => r.GetCurrentWeek(), Times.Once);
            mockShiftRepo.Verify(r => r.GetShiftsForWeek(weekStart), Times.Once);
        }
    }
}