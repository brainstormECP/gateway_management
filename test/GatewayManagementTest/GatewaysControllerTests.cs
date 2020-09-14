using System;
using Xunit;
using GatewayManagement.Controllers;
using GatewayManagement.Data;
using Moq;
using GatewayManagement.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace GatewayManagementTest
{
    public class GatewayControllerTests
    {

        [Fact]
        public async void TestGetAllGateways()
        {
            // Arrange
            var gateway = new Gateway { Name = "BBB" };
            var data = new List<Gateway>
            {
                gateway,
                new Gateway {Id=new Guid(), Name = "ZZZ", IPv4="127.0.0.1", SerialNumber="qwe123" },
                new Gateway { Id=new Guid(), Name = "asd", IPv4="127.0.0.1", SerialNumber="qdsfs" },
            }.AsQueryable();
            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_db");
            var db = new GatewayDbContext(options.Options);
            db.AddRange(data);
            db.SaveChanges();
            var loggerMock = new Mock<ILogger<GatewayController>>();
            var controller = new GatewayController(db, loggerMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.Contains(gateway, result);
        }
    }
}
