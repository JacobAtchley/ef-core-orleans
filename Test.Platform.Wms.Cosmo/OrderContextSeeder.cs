using System;
using System.Collections.Generic;
using System.Linq;
using Test.Platform.Wms.Core.Data;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.Cosmo.Contexts;

namespace Test.Platform.Wms.Cosmo
{
    public static class OrderContextSeeder
    {
        public static void Init(OrderContext context, int numberOfOrders)
        {
            context.Database.EnsureCreated();

            if(context.Orders.Count() > 0)
            //if (context.Orders.Any())
            {
                return;
            }

            var orders = Enumerable.Range(0, numberOfOrders)
                .Select(_ => GenerateOrder())
                .ToList();
            
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        private static Order GenerateOrder()
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                BillTo = GetAddress(),
                ShipTo = GetAddress(),
                CreateDate = DateTimeOffset.UtcNow.AddDays(Faker.RandomNumber.Next(30) * -1),
                OrderNumber = $"PO-{Faker.Finance.Coupon()}",
                Lines = GetLines(),
            };

            return order;
        }

        private static List<OrderLine> GetLines()
        {
            return Enumerable
                .Range(0, Faker.RandomNumber.Next(5, 25))
                .Select(x =>
                {
                    var item = StaticData.GetRandomItem();
                    var orderLineItem = new OrderLineItem
                    {
                        ItemDescription = item.Description,
                        ItemId = item.Id,
                        ItemName = item.Name
                    };
                    
                    return new OrderLine
                    {
                        Id = Guid.NewGuid(),
                        Item = orderLineItem,
                        Quantity = Faker.RandomNumber.Next(1, 100),
                        ItemId = item.Id
                    };
                })
                .ToList();
        }

        private static Address GetAddress()
        {
            return new Address
            {
                City = Faker.Address.City(),
                State = Faker.Address.UsState(),
                Street = Faker.Address.StreetAddress(),
                Zip = Faker.Address.ZipCode()

            };
        }
    }
}