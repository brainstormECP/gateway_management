using System;
using Xunit;
using GatewayManagement.Controllers;
using GatewayManagement.Data;
using Moq;
using GatewayManagement.Models;
using GatewayManagement.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace GatewayManagementTest
{
    public class GatewayRepositoryTest
    {

        [Fact]
        public async void TestFindAll()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_findall");
            var db = new GatewayDbContext(options.Options);
            // db.RemoveRange(db.Gateways);
            // await db.SaveChangesAsync();

            var gateway = new Gateway { Id = 1, Name = "BBB", IPv4 = "192.168.4.12", SerialNumber = "sdsd" };
            var data = new List<Gateway>
            {
                gateway,
                new Gateway {Id=2, Name = "ZZZ", IPv4="127.0.0.1", SerialNumber="qwe123" },
                new Gateway { Id=3, Name = "asd", IPv4="127.0.0.1", SerialNumber="qdsfs" },
            };

            db.AddRange(data);
            db.SaveChanges();
            var repo = new GatewayRepository(db);

            // Act
            var result = await repo.FindAll();

            // Assert
            Assert.Contains(gateway, result);
        }

        [Fact]
        public async void TestFindById()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_findbyid");
            var db = new GatewayDbContext(options.Options);
            // db.RemoveRange(db.Gateways);
            // await db.SaveChangesAsync();

            var gateway = new Gateway { Id = 1, Name = "BBB", IPv4 = "192.168.4.12", SerialNumber = "sdsd" };
            var data = new List<Gateway>
            {
                gateway,
                new Gateway {Id=2, Name = "ZZZ", IPv4="127.0.0.1", SerialNumber="qwe123" },
                new Gateway { Id=3, Name = "asd", IPv4="127.0.0.1", SerialNumber="qdsfs" },
            }.AsQueryable();

            db.AddRange(data);
            await db.SaveChangesAsync();
            var repo = new GatewayRepository(db);

            // Act
            var result = await repo.FindById(1);

            // Assert
            Assert.Equal("BBB", result.Name);
        }

        [Fact]
        public async void TestCreate()
        {
            // Arrange
            var gateway = new Gateway { Name = "Asdsd", IPv4 = "192.168.4.12", SerialNumber = "sdsd" };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_db");
            var db = new GatewayDbContext(options.Options);
            db.RemoveRange(db.Gateways);
            db.SaveChanges();
            var repo = new GatewayRepository(db);

            // Act
            var result = await repo.Create(gateway);

            // Assert
            Assert.True(result.Status);
        }

        [Fact]
        public async void TestUpdate()
        {
            // Arrange
            var gateway = new Gateway { Id = 1, Name = "wee", IPv4 = "192.168.4.12", SerialNumber = "sdsd" };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_update");
            var db = new GatewayDbContext(options.Options);
            // db.RemoveRange(db.Gateways);
            // await db.SaveChangesAsync();
            db.Add(gateway);
            db.SaveChanges();
            var repo = new GatewayRepository(db);

            // Act
            gateway.IPv4 = "192.168.4.13";
            var result = await repo.Update(gateway);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("192.168.4.13", ((Gateway)result.Entity).IPv4);
        }

        [Fact]
        public async void TestDelete()
        {
            // Arrange
            var gateway = new Gateway { Id = 1, Name = "wee", IPv4 = "192.168.4.12", SerialNumber = "sdsd" };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_delete");
            var db = new GatewayDbContext(options.Options);
            db.Add(gateway);
            db.SaveChanges();
            var repo = new GatewayRepository(db);

            // Act
            gateway.IPv4 = "192.168.4.13";
            var result = await repo.Delete(1);

            // Assert
            Assert.True(result.Status);
        }

        [Fact]
        public async void TestDuplicateSerialNumber()
        {
            // Arrange
            var gateway = new Gateway { Id = 1, Name = "wee", IPv4 = "192.168.4.1", SerialNumber = "sdsd" };
            var gateway2 = new Gateway { Id = 2, Name = "qwe", IPv4 = "192.168.4.2", SerialNumber = "sdsd" };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_serial");
            var db = new GatewayDbContext(options.Options);
            db.Add(gateway);
            db.SaveChanges();

            var repo = new GatewayRepository(db);

            // Act
            var result = await repo.Create(gateway2);

            // Assert
            Assert.False(result.Status);
        }

        [Fact]
        public async void TestNoMoreThan10Devices()
        {
            // Arrange
            var gateway = new Gateway
            {
                Id = 1,
                Name = "wee",
                IPv4 = "192.168.4.12",
                SerialNumber = "sdsd",
                Devices = new List<Device>
                {
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                    new Device{UID=1},
                }
            };


            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("gateway_test_devices");
            var db = new GatewayDbContext(options.Options);

            var repo = new GatewayRepository(db);

            // Act
            var result = await repo.Create(gateway);

            // Assert
            Assert.False(result.Status);
        }
    }
}
