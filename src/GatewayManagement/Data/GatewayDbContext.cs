using Microsoft.EntityFrameworkCore;

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
    }
    public DbSet<Gateway> Gateways { get; set; }
    public DbSet<Device> Devices { get; set; }
}