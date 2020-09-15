using Xunit;
using GatewayManagement.Controllers;
using GatewayManagement.Data;
using Moq;
using GatewayManagement.Models;
using GatewayManagement.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GatewayManagementTest
{
    public class GatewayControllerTest
    {

        [Fact]
        public async void TestGet()
        {
            // Arrange
            var gateway = new Gateway { Id = 1, Name = "BBB", IPv4 = "192.168.4.12", SerialNumber = "sdsd" };
            var data = new List<Gateway>
            {
                gateway,
                new Gateway {Id=2, Name = "ZZZ", IPv4="127.0.0.1", SerialNumber="qwe123" },
                new Gateway { Id=3, Name = "asd", IPv4="127.0.0.1", SerialNumber="qdsfs" },
            };
            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_get");
            var db = new GatewayDbContext(options.Options);
            db.AddRange(data);
            db.SaveChanges();
            var repo = new GatewayRepository(db);
            var loggerMock = new Mock<ILogger<GatewayController>>();
            var controller = new GatewayController(repo, loggerMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.Contains(gateway, result);
        }

    }
}
