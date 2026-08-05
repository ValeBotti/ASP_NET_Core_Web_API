using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}

    public DbSet<UidSid> UidSids { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Menu> Menus { get; set; }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>().OwnsOne(m => m.Location);

        modelBuilder.Entity<Order>().OwnsOne(o => o.DeliveryLocation);
        modelBuilder.Entity<Order>().OwnsOne(o => o.CurrentPosition);
    }

}