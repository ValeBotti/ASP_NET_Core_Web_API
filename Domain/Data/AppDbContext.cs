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

        modelBuilder.Entity<UidSid>(entity =>
        {
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Uid).HasColumnName("user_id");

            entity.ToTable("uid_sid",t =>
            {
                t.HasCheckConstraint("CK_uid_sid_user_id_positive", "user_id >= 0");
            });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.FirstName).HasColumnName("first_name");
            entity.Property(u => u.LastName).HasColumnName("last_name");
            entity.Property(u => u.CardFullName).HasColumnName("card_full_name");
            entity.Property(u => u.CardNumber).HasColumnName("card_number");
            entity.Property(u => u.CardExpireMonth).HasColumnName("card_expire_month");
            entity.Property(u => u.CardExpireYear).HasColumnName("card_expire_year");
            entity.Property(u => u.CardCVV).HasColumnName("card_cvv");

            entity.ToTable("user", t =>
            {
                t.HasCheckConstraint("CK_user_id_positive", "id > 0");
                t.HasCheckConstraint("CK_user_card_expire_month", "card_expire_month BETWEEN 1 AND 12");
            });
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.Property(m => m.Id).HasColumnName("id");
            entity.Property(m => m.Name).HasColumnName("name");
            entity.Property(m => m.Price).HasColumnName("price");
            entity.OwnsOne(o => o.Location, nav =>
            {
                nav.Property(p => p.Lat).HasColumnName("location_lat");
                nav.Property(p => p.Lng).HasColumnName("location_lng");
            });
            entity.Property(m => m.ImageVersion).HasColumnName("image_version");
            entity.Property(m => m.Image).HasColumnName("image");
            entity.Property(m => m.ShortDescription).HasColumnName("short_description");
            entity.Property(m => m.LongDescription).HasColumnName("long_description");
            entity.Property(m => m.DeliveryTime).HasColumnName("delivery_time");

            entity.ToTable("menu", t =>
            {
                t.HasCheckConstraint("CK_menu_id_positive", "id > 0");
                t.HasCheckConstraint("CK_menu_price_positive", "price >= 0");
                t.HasCheckConstraint("CK_menu_image_version_positive", "image_version >= 0");
                t.HasCheckConstraint("CK_menu_lat_range", "location_lat BETWEEN -90 AND 90");
                t.HasCheckConstraint("CK_menu_lng_range", "location_lng BETWEEN -180 AND 180");
            });
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(o => o.Id).HasColumnName("id");
            entity.Property(o => o.Uid).HasColumnName("user_id");
            entity.Property(o => o.Mid).HasColumnName("menu_id");
            entity.Property(o => o.CreationTimestamp).HasColumnName("creation_timestamp");
            entity.Property(o => o.Status).HasColumnName("status");
            entity.Property(o => o.DeliveryTimestamp).HasColumnName("delivery_timestamp");
            entity.OwnsOne(o => o.CurrentPosition, nav =>
            {
                nav.Property(p => p.Lat).HasColumnName("current_position_lat");
                nav.Property(p => p.Lng).HasColumnName("current_position_lng");
            });

            entity.ToTable("order", t =>
            {
                t.HasCheckConstraint("CK_order_id_positive", "id > 0");
                t.HasCheckConstraint("CK_order_user_id_positive", "user_id > 0");
                t.HasCheckConstraint("CK_order_menu_id_positive", "menu_id > 0");

                t.HasCheckConstraint("CK_order_lat_range", "current_position_lat BETWEEN -90 AND 90");
                t.HasCheckConstraint("CK_order_lng_range", "current_position_lng BETWEEN -180 AND 180");

                t.HasCheckConstraint("CK_order_status_valid",
                    "status IN ('ON_DELIVERY', 'COMPLETED')");

                t.HasCheckConstraint("CK_order_delivery_timestamp_logic",
                    "(status = 'ON_DELIVERY' AND delivery_timestamp IS NULL) OR " +
                    "(status = 'COMPLETED' AND delivery_timestamp IS NOT NULL)");

                t.HasCheckConstraint("CK_order_delivery_after_creation",
                    "delivery_timestamp >= creation_timestamp");
            });
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

}