using Microsoft.EntityFrameworkCore;
using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.DataAccess;

public class YunusExpressDbContext : DbContext
{
    public YunusExpressDbContext(DbContextOptions<YunusExpressDbContext> option) : base(option) { }

    public DbSet<Courier> Couriers { get; set; }
    public DbSet<DeliveryZone> DeliveryZones { get; set; }
    public DbSet<DeliveryZoneType> DeliveryZoneTypes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Receiver> Receivers { get; set; }
    public DbSet<Sender> Senders { get; set; }
    public DbSet<ServiceTypes> ServiceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YunusExpressDbContext).Assembly);
    }
}

