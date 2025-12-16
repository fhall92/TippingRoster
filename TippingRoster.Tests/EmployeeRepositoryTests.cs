using System;
using System.Collections.Generic;
using Xunit;
using TippingRoster.Infrastructure.Data;
using TippingRoster.Infrastructure.Repositories;
using TippingRoster.Domain.Entities;

namespace TippingRoster.Tests.Repositories
{
    public class EmployeeRepositoryTests
    {
        [Fact]
        public void GetAll_ReturnsAllEmployeesFromContext()
        {
            // Arrange
            var ctx = new InMemoryDataContext();
            var e1 = new Employee(Guid.NewGuid(), "Alice");
            var e2 = new Employee(Guid.NewGuid(), "Bob");
            ctx.Employees.AddRange(new[] { e1, e2 });

            var repo = new EmployeeRepository(ctx);

            // Act
            var result = repo.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(e1, result);
            Assert.Contains(e2, result);
        }

        [Fact]
        public void GetById_ReturnsEmployee_WhenExists()
        {
            // Arrange
            var ctx = new InMemoryDataContext();
            var e = new Employee(Guid.NewGuid(), "Charlie");
            ctx.Employees.Add(e);

            var repo = new EmployeeRepository(ctx);

            // Act
            var fetched = repo.GetById(e.Id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(e.Id, fetched!.Id);
            Assert.Equal("Charlie", fetched.Name);
        }

        [Fact]
        public void GetById_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var ctx = new InMemoryDataContext();
            var repo = new EmployeeRepository(ctx);

            // Act
            var fetched = repo.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(fetched);
        }
    }
}