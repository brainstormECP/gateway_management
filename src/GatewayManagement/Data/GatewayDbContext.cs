using System;
using System.Collections.Generic;
using GatewayManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GatewayManagement.Data
{
    public class GatewayDbContext : DbContext
    {
        public GatewayDbContext(DbContextOptions<GatewayDbContext> options)
                : base(options)
        {

        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gateway>().HasIndex(g => g.SerialNumber).IsUnique(true);
            modelBuilder.Entity<Device>().HasIndex(g => g.UID).IsUnique(true);

            //Demo data
            modelBuilder.Entity<Gateway>().HasData(new List<Gateway>{
                new Gateway{Id=1,Name="Gateway 1", SerialNumber="123qwe", IPv4="192.168.1.12"},
                new Gateway{Id=2,Name="Gateway 2", SerialNumber="qwe33", IPv4="192.168.1.13"},
                new Gateway{Id=3,Name="Gateway 3", SerialNumber="sgad", IPv4="192.168.1.14"},
                new Gateway{Id=4,Name="Gateway 4", SerialNumber="dsfasga", IPv4="192.168.1.15"}
            });
            modelBuilder.Entity<Device>().HasData(new List<Device>{
                new Device{Id=1,UID=343,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=1},
                new Device{Id=2,UID=324234,Vendor="Samsung", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=1},
                new Device{Id=3,UID=234,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Offline, GatewayId=1},
                new Device{Id=4,UID=234234,Vendor="Samsung", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=1},
                new Device{Id=5,UID=2423,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Offline, GatewayId=1},
                new Device{Id=6,UID=122343,Vendor="Samsung", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=1},
                new Device{Id=7,UID=442,Vendor="Samsung", CreatedDate= DateTime.Now, Status=Status.Offline, GatewayId=1},
                new Device{Id=8,UID=452,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=1},
                new Device{Id=9,UID=45542,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=2},
                new Device{Id=10,UID=46652,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=2},
                new Device{Id=11,UID=64,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=2},
                new Device{Id=12,UID=55737,Vendor="Samsung", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=3},
                new Device{Id=13,UID=677327,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Offline, GatewayId=3},
                new Device{Id=14,UID=556,Vendor="Samsung", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=3},
                new Device{Id=15,UID=562,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Offline, GatewayId=4},
                new Device{Id=16,UID=678,Vendor="Samsung", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=4},
                new Device{Id=17,UID=573,Vendor="Samsung", CreatedDate= DateTime.Now, Status=Status.Offline, GatewayId=4},
                new Device{Id=18,UID=70665,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=4},
                new Device{Id=19,UID=46748,Vendor="Apple", CreatedDate= DateTime.Now, Status=Status.Online, GatewayId=4},
            });
        }
        public virtual DbSet<Gateway> Gateways { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
    }
}