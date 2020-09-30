using System;
using System.Linq;
using Test.Platform.Wms.Core.Data;
using Test.Platform.Wms.Sql.Contexts;

namespace Test.Platform.Wms.Api
{
    public static class InventoryContextSeeder
    {
        public static void Init(InventoryContext context)
        {
            context.Database.EnsureCreated();

            if (context.Items.Any())
            {
                return;
            }

            context.Items.AddRange(StaticData.Items);
            context.SaveChanges();
            
            context.Inventories.AddRange(StaticData.Inventories);
            context.SaveChanges();
        }
    }
}