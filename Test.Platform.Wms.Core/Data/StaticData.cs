using System;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Core.Data
{
    public class StaticData
    {
        public static readonly Item[] Items = new[]
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
                Name = "Thicc Orange Pumpkin"
            },
            new Item
            {
                Description = "A big red gourd",
                Id = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D4"),
                Name = "Thicc Red Pumpkin"
            },
            new Item
            {
                Description = "A big yellow gourd",
                Id = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D5"),
                Name = "Thicc Yellow Pumpkin"
            },
            new Item
            {
                Description = "A small cold beer",
                Id = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D6"),
                Name = "Wee Lil Beer"
            },
            new Item
            {
                Description = "A medium cold beer",
                Id = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D7"),
                Name = "Lil Beer"
            },
            new Item
            {
                Description = "A large cold beer",
                Id = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D8"),
                Name = "Big Ole Beer"
            }
        };

        public static readonly Inventory[] Inventories = new[]
        {
            new Inventory
            {
                Count = 1000,
                Id = Guid.Parse("9D02439A-7CB0-4132-91EB-0176F509D8D3"),
                ItemId = Items[0].Id
            },

            new Inventory
            {
                Count = 5000,
                Id = Guid.Parse("11E201A1-3903-4821-AAAD-1EC69EF76280"),
                ItemId = Items[0].Id
            }
        };

        private static readonly Random ItemRandomizer = new Random();

        public static Item GetRandomItem()
        {
            var index = ItemRandomizer.Next(0, Items.Length -1);

            return Items[index];
        }
    }
}