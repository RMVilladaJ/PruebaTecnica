using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Shared.Entities;

namespace PruebaTecnica.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 🔹 Relación entre Product y Supplier
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)   // Un producto tiene un proveedor
                .WithMany(s => s.Products)      // Un proveedor puede tener muchos productos
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);  // 
        }
    }
}
