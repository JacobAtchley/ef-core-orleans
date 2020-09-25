using Microsoft.EntityFrameworkCore;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Sql.Contexts
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base (options)
        {
            
        }
        
        public DbSet<Item> Items { get; set; }
        
        public DbSet<Inventory> Inventories { get; set; }
    }
}