using System;
using System.Linq;
using Test.Platform.Wms.Core.Models;
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

            var items = new[]
            {
                new Item
                {
                    Description = "A big ole box",
                    Id = Guid.Parse("C6C2D42C-EF2B-4870-89B7-155947A6A33C"),
                    Name = "Large Box"
                },
                new Item
                {
                    Description = "A big orange gourd",
                    Id = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D3"),
                    Name = "Thicc Pumpkin"
                }
            };
            
            context.Items.AddRange(items);
            context.SaveChanges();

            var inventories = new[]
            {
                new Inventory
                {
                    Count = 100,
                    Id = Guid.Parse("9D02439A-7CB0-4132-91EB-0176F509D8D3"),
                    ItemId = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D3")
                },

                new Inventory
                {
                    Count = 25,
                    Id = Guid.Parse("11E201A1-3903-4821-AAAD-1EC69EF76280"),
                    ItemId = Guid.Parse("C6C2D42C-EF2B-4870-89B7-155947A6A33C")
                }
            };
            
            context.Inventories.AddRange(inventories);
            context.SaveChanges();
        }
    }
}