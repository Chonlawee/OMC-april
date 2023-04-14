using Microsoft.EntityFrameworkCore;
using OMC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;


namespace OMC.Data
{
   
    public class OMCContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ILogger<OMCContext> _logger;
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Machine> Machine { get; set; }
        public DbSet<MachineStock> machineStocks { get; set; }
    
        //public DbSet<PaymentDetail> paymentDetail { get; set; }

        public OMCContext(DbContextOptions<OMCContext>options ,ILogger<OMCContext> logger) : base(options) 
        {
            _logger = logger;
            _logger.LogInformation("OMCContext created.");

        }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Order>()
               .Property(o => o.Created)
               .HasColumnType("datetime")
               .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Order>()
                .Property(o => o.Modified)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Order>()
                .Property(o => o.Deleted)
                .HasColumnType("datetime2")
                .HasDefaultValue(null);

            modelBuilder.Entity<Recipe>()
                 .Property(o => o.Created)
                 .HasColumnType("datetime")
                 .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Recipe>()
                .Property(o => o.Modified)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Recipe>()
                .Property(o => o.Deleted)
                .HasColumnType("datetime2")
                .HasDefaultValue(null);

            modelBuilder.Entity<Product>()
             .Property(o => o.Created)
             .HasColumnType("datetime")
             .HasDefaultValueSql("getdate()");


            modelBuilder.Entity<Product>()
            .Property(o => o.Modified)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Product>()
                .Property(o => o.Deleted)
                .HasColumnType("datetime2")
                .HasDefaultValue(null);

            modelBuilder.Entity<Machine>()
                .Property(o => o.Created)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Machine>()
                .Property(o => o.Modified)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Machine>()
                .Property(o => o.Deleted)
                .HasColumnType("datetime2")
                .HasDefaultValue(null);

            modelBuilder.Entity<MachineStock>()
                .Property(o => o.Created)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<MachineStock>()
                .Property(o => o.Modified)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<MachineStock>()
                .Property(o => o.Deleted)
                .HasColumnType("datetime2")
                .HasDefaultValue(null);
    

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
               .HasForeignKey(o => o.ProductID);

            modelBuilder.Entity<Order>()
               .HasOne(o => o.User)
               .WithMany()
              .HasForeignKey(o => o.UserID);

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Product)  // Reference to the "Recipe" navigation property on the "Product" entity
                .WithMany()
                .HasForeignKey(r => r.ProductID);

            modelBuilder.Entity<Recipe>()
                .HasIndex(r => r.ProductID);

            modelBuilder.Entity<MachineStock>()
                .HasOne(m => m.Machine)
                .WithMany()
                .HasForeignKey(m => m.MachineID);

        }

    }   

    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
