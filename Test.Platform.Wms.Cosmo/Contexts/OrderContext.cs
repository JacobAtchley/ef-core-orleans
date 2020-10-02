using Microsoft.EntityFrameworkCore;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Cosmo.Contexts
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            
        }
        
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().ToContainer("Orders");
            modelBuilder.Entity<Order>().OwnsOne(order => order.BillTo);
            modelBuilder.Entity<Order>().OwnsOne(order => order.ShipTo);
            modelBuilder.Entity<Order>().OwnsMany(order => order.Lines,
                lines =>
                {
                    lines.OwnsOne(line => line.Item);
                });

        }
    }
}