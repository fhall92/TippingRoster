using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TippingRoster.Api.Controllers;
using TippingRoster.Application.Interfaces;
using TippingRoster.Domain.Entities;
using TippingRoster.Application.Queries;

namespace TippingRoster.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        [Fact]
        public void GetAll_ReturnsOk_WithEmployeeList()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee(Guid.NewGuid(), "Alice"),
                new Employee(Guid.NewGuid(), "Bob")
            };

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(employees);

            var controller = new EmployeesController(mockRepo.Object);

            // Act
            var result = controller.GetAll(new GetEmployeesQuery());

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(employees, ok.Value); // controller returns repository list directly
            mockRepo.Verify(r => r.GetAll(), Times.Once);
        }
    }
}