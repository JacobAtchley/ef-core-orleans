using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.Sql.Abstractions;
using Test.Platform.Wms.Sql.Contexts;

namespace Test.Platform.Wms.Sql.Implementations
{
    public class InventoryRepository : AbstractDbContextCrudRepo<Inventory, InventoryContext>, IInventoryRepository
    {
        public InventoryRepository(InventoryContext context) : base(context)
        {
        }

        public Task<List<Inventory>> GetByItemId(Guid itemId, CancellationToken cancellationToken)
        {
            return Context.Inventories.Where(x => x.ItemId == itemId).ToListAsync(cancellationToken);
        }
    }
}