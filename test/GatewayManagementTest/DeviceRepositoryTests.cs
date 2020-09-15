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
    public class DeviceRepositoryTest
    {

        [Fact]
        public async void TestFindAll()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("device_test_findall");
            var db = new GatewayDbContext(options.Options);

            var gateway = new Gateway { Id = 1, Name = "ZZZ", IPv4 = "127.0.0.1", SerialNumber = "qwe123" };
            var device = new Device { Id = 1, CreatedDate = new DateTime(), Status = Status.Online, UID = 1, Vendor = "Vendor", GatewayId = 1 };
            var data = new List<Device>
            {
                device,
                new Device { Id = 2, CreatedDate=new DateTime(), Status=Status.Online, UID=1, Vendor="Vendor", GatewayId=1 },
                new Device { Id = 3, CreatedDate=new DateTime(), Status=Status.Online, UID=1, Vendor="Vendor", GatewayId=1 }
            };
            db.Add(gateway);
            db.AddRange(data);
            db.SaveChanges();
            var repo = new DeviceRepository(db);

            // Act
            var result = await repo.FindAll();

            // Assert
            Assert.Contains(device, result);
        }

        [Fact]
        public async void TestFindById()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("device_test_findbyid");
            var db = new GatewayDbContext(options.Options);

            var gateway = new Gateway { Id = 1, Name = "ZZZ", IPv4 = "127.0.0.1", SerialNumber = "qwe123" };
            var device = new Device { Id = 1, CreatedDate = new DateTime(), Status = Status.Online, UID = 1, Vendor = "Vendor", Gateway = gateway };
            db.Add(device);
            await db.SaveChangesAsync();
            var repo = new DeviceRepository(db);

            // Act
            var result = await repo.FindById(1);

            // Assert
            Assert.Equal(1, result.UID);
        }

        [Fact]
        public async void TestCreate()
        {
            // Arrange
            var gateway = new Gateway { Id = 1, Name = "ZZZ", IPv4 = "127.0.0.1", SerialNumber = "qwe123" };
            var device = new Device { Id = 1, CreatedDate = DateTime.Now, Status = Status.Online, UID = 1, Vendor = "Vendor", GatewayId = 1 };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("device_test_db");
            var db = new GatewayDbContext(options.Options);
            db.Add(gateway);
            db.SaveChanges();
            var repo = new DeviceRepository(db);

            // Act
            var result = await repo.Create(device);

            // Assert
            Assert.True(result.Status);

        }

        [Fact]
        public async void TestUpdate()
        {
            // Arrange
            var gateway = new Gateway { Id = 1, Name = "ZZZ", IPv4 = "127.0.0.1", SerialNumber = "qwe123" };
            var device = new Device { Id = 1, CreatedDate = new DateTime(), Status = Status.Online, UID = 1, Vendor = "Vendor", Gateway = gateway };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("device_test_update");
            var db = new GatewayDbContext(options.Options);
            // db.RemoveRange(db.Gateways);
            // await db.SaveChangesAsync();
            db.Add(device);
            db.SaveChanges();
            var repo = new DeviceRepository(db);

            // Act
            device.Vendor = "Vendor 1";
            var result = await repo.Update(device);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Vendor 1", ((Device)result.Entity).Vendor);
        }

        [Fact]
        public async void TestDelete()
        {
            // Arrange
            var gateway = new Gateway { Id = 1, Name = "ZZZ", IPv4 = "127.0.0.1", SerialNumber = "qwe123" };
            var device = new Device { Id = 1, CreatedDate = new DateTime(), Status = Status.Online, UID = 1, Vendor = "Vendor", Gateway = gateway };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("device_test_delete");
            var db = new GatewayDbContext(options.Options);
            db.Add(device);
            db.SaveChanges();
            var repo = new DeviceRepository(db);

            // Act
            var result = await repo.Delete(1);

            // Assert
            Assert.True(result.Status);
        }

        [Fact]
        public async void TestDuplicateUID()
        {
            // Arrange
            var gateway = new Gateway { Id = 1, Name = "ZZZ", IPv4 = "127.0.0.1", SerialNumber = "qwe123" };
            var device = new Device { Id = 1, CreatedDate = new DateTime(), Status = Status.Online, UID = 1, Vendor = "Vendor", Gateway = gateway };
            var device1 = new Device { Id = 2, CreatedDate = new DateTime(), Status = Status.Online, UID = 1, Vendor = "Vendor", Gateway = gateway };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("device_test_uid");
            var db = new GatewayDbContext(options.Options);
            db.Add(device);
            db.SaveChanges();

            var repo = new DeviceRepository(db);

            // Act
            var result = await repo.Create(device1);

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
                }
            };
            var device = new Device { Id = 1, CreatedDate = new DateTime(), Status = Status.Online, UID = 2, Vendor = "Vendor", Gateway = gateway };

            var options = new DbContextOptionsBuilder<GatewayDbContext>().UseInMemoryDatabase("device_test_devices");
            var db = new GatewayDbContext(options.Options);

            var repo = new DeviceRepository(db);

            // Act
            var result = await repo.Create(device);

            // Assert
            Assert.False(result.Status);
        }
    }
}
