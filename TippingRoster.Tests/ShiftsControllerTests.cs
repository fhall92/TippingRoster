using System;
using Xunit;
using TippingRoster.Api.Controllers;
using TippingRoster.Application.Interfaces;
using TippingRoster.Domain.Entities;
using TippingRoster.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TippingRoster.Tests.Controllers
{
    public class ShiftsControllerTests
    {
        [Fact]
        public void Create_AddsShiftToRepository_UsingMoq()
        {
            // Arrange
            var mockRepo = new Mock<IShiftRepository>();
            Shift? captured = null;
            mockRepo
                .Setup(r => r.Add(It.IsAny<Shift>()))
                .Callback<Shift>(s => captured = s);

            var controller = new ShiftsController(mockRepo.Object);

            var cmd = new CreateShiftCommand
            {
                EmployeeId = Guid.NewGuid(),
                Date = new DateOnly(2025, 12, 15),
                StartTime = new DateTime(2025, 12, 15, 9, 0, 0),
                EndTime = new DateTime(2025, 12, 15, 13, 0, 0)
            };

            // Act
            var result = controller.Create(cmd);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.NotNull(captured);
            Assert.Equal(cmd.EmployeeId, captured!.EmployeeId);
            Assert.Equal(cmd.Date, captured.Date);
            Assert.Equal(cmd.StartTime, captured.StartTime);
            Assert.Equal(cmd.EndTime, captured.EndTime);

            mockRepo.Verify(r => r.Add(It.IsAny<Shift>()), Times.Once);
        }

        [Fact]
        public void Update_ReplacesExistingShift_UsingMoq()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existing = new Shift(id, Guid.NewGuid(), new DateOnly(2025, 12, 15),
                new DateTime(2025, 12, 15, 8, 0, 0),
                new DateTime(2025, 12, 15, 12, 0, 0));

            var mockRepo = new Mock<IShiftRepository>();
            mockRepo.Setup(r => r.GetById(id)).Returns(existing);

            Shift? updatedParam = null;
            mockRepo
                .Setup(r => r.Update(It.IsAny<Shift>()))
                .Callback<Shift>(s => updatedParam = s);

            var controller = new ShiftsController(mockRepo.Object);

            var updateCmd = new UpdateShiftCommand
            {
                EmployeeId = existing.EmployeeId,
                Date = existing.Date,
                StartTime = new DateTime(2025, 12, 15, 13, 0, 0),
                EndTime = new DateTime(2025, 12, 15, 17, 0, 0)
            };

            // Act
            var result = controller.Update(id, updateCmd);

            // Assert
            Assert.IsType<OkResult>(result);

            Assert.NotNull(updatedParam);
            Assert.Equal(id, updatedParam!.Id);
            Assert.Equal(updateCmd.EmployeeId, updatedParam.EmployeeId);
            Assert.Equal(updateCmd.Date, updatedParam.Date);
            Assert.Equal(updateCmd.StartTime, updatedParam.StartTime);
            Assert.Equal(updateCmd.EndTime, updatedParam.EndTime);

            mockRepo.Verify(r => r.GetById(id), Times.Once);
            mockRepo.Verify(r => r.Update(It.IsAny<Shift>()), Times.Once);
        }
    }
}