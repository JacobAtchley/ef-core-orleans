using System.Diagnostics;
using System.Linq;
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
            System.Console.WriteLine("Starting....");

            const string root = "http://localhost:5000/api/v1/inventory";

            var grain = RestService.For<IInventoryClient>($"{root}/grain/");
            var service = RestService.For<IInventoryClient>($"{root}/service/");
            var grainPersistence = RestService.For<IInventoryClient>($"{root}/grain/persistence/");

            var pumpkinId = Guid.Parse("2A50AA6C-8FE8-4C7A-BB93-731F0BD637D3");
            
            System.Console.WriteLine("~*~*~*~*~*~* Grain ~*~*~*~*~*~*~*~*");
            await DecrementInventory(pumpkinId, grain);
            System.Console.WriteLine(Environment.NewLine);
            
            System.Console.WriteLine("~*~*~*~*~*~* Grain Persistence ~*~*~*~*~*~*~*~*");
            await DecrementInventory(pumpkinId, service);
            System.Console.WriteLine(Environment.NewLine);
            
            System.Console.WriteLine("~*~*~*~*~*~* Service EF  ~*~*~*~*~*~*~*~*");
            await DecrementInventory(pumpkinId, grainPersistence);
            System.Console.WriteLine(Environment.NewLine);
        }

        static async Task DecrementInventory(Guid pumpkinId, IInventoryClient client)
        {
            var currentInventory = await client.IncrementInventoryAsync(pumpkinId, 1000, 0);
            
            LogOperation(currentInventory);

            var tasks = Enumerable.Range(1, 10)
                .Select(async x => {
                    var sw = new Stopwatch();
                    sw.Start();
                    var inv = await client.DecrementInventoryAsync(pumpkinId, x, x);
                    sw.Stop();
                    System.Console.WriteLine($"*************** Index: {x}. Inventory Count: {inv.Count}. Duration: {sw.Elapsed} **************");
                });

            await Task.WhenAll(tasks);
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