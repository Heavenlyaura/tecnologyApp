using Microsoft.EntityFrameworkCore;
using tecnologyApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         foreach (var property in modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal)))
    {
        property.SetColumnType("decimal(18,2)");
    }


        base.OnModelCreating(modelBuilder);

   
        ConfigureDecimalPrecisions(modelBuilder);

   
        ConfigureRelationships(modelBuilder);

    
        SeedInitialData(modelBuilder);
    }

    private void ConfigureDecimalPrecisions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseItem>()
            .Property(pi => pi.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Purchase>()
            .Property(p => p.Total)
            .HasPrecision(18, 2);
    }

    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
      
       modelBuilder.Entity<PurchaseItem>(entity =>
    {
     
        entity.HasKey(pi => new { pi.PurchaseId, pi.ProductId });

      
        entity.HasOne(pi => pi.Purchase)
            .WithMany(p => p.PurchaseItems)
            .HasForeignKey(pi => pi.PurchaseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

      
        entity.HasOne(pi => pi.Product)
            .WithMany() // O WithMany(p => p.PurchaseItems) 
            .HasForeignKey(pi => pi.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    });


        // modelBuilder.Entity<Product>()
        //     .HasMany(p => p.PurchaseItems)
        //     .WithOne(pi => pi.Product);
    }

    private void SeedInitialData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "demo_seller",
            Password = "1234", 
            Role = "Seller"
        });
        
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Gaming Laptop", Price = 1200m, Quantity = 5, 
                         SellerUsername = "demo_seller", Image = "images/laptop.jpg", UserId = 1 },
            new Product { Id = 2, Name = "Wireless Headphones", Price = 250m, Quantity = 10, 
                         SellerUsername = "demo_seller", Image = "images/auriculares.webp", UserId = 1 },
            new Product { Id = 3, Name = "27'' 4K Monitor", Price = 600m, Quantity = 3, 
                         SellerUsername = "demo_seller", Image = "images/monitor.png", UserId = 1 },
            new Product { Id = 4, Name = "RGB Mechanical Keyboard", Price = 150m, Quantity = 15, 
                         SellerUsername = "demo_seller", Image = "images/teclado.webp", UserId = 1 },
            new Product { Id = 5, Name = "Gaming Mouse", Price = 90m, Quantity = 20, 
                         SellerUsername = "demo_seller", Image = "images/mouse.jpg", UserId = 1 }
        );
    }
}