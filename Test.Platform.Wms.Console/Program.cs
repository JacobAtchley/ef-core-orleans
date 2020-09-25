using System;
using System.Text.Json;
using System.Threading.Tasks;
using Refit;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rest = RestService.For<IInventoryClient>("http://localhost:5000/api/v1/inventory");

            var pumpkinId = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D3");

            var currentInventory = await rest.IncrementInventoryAsync(pumpkinId, 1000);
            LogOperation(currentInventory);
        }

        static void LogOperation(Inventory inventory)
        {
            var json = JsonSerializer.Serialize(inventory, typeof(Inventory), new JsonSerializerOptions { WriteIndented = true});

            System.Console.WriteLine($@"
----------{DateTime.Now}-----------------------
{json}
--------------------------------");
        }
    }
}