using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.EntityFramework.Abstractions;
using Test.Platform.Wms.Sql.Contexts;

namespace Test.Platform.Wms.Sql.Implementations
{
    public class ItemRepository : AbstractDbContextCrudRepo<Item, InventoryContext>, IItemRepository
    {
        public ItemRepository(InventoryContext context) : base(context)
        {
        }
    }
}