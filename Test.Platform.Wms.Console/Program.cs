using System.Diagnostics;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Refit;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.Core.Data;

namespace Test.Platform.Wms.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Starting....");

            const string root = "http://localhost:5000/api/v1/inventory";

            // await DecrementInventory(RestService.For<IInventoryClient>($"{root}/grain/"),
            //     "Grain",
            //     1000,
            //     true,
            //     false);
            //
            // await DecrementInventory(RestService.For<IInventoryClient>($"{root}/grain/"),
            //     "Grain",
            //     1000,
            //     false,
            //     false);
            //
            // await DecrementInventory(RestService.For<IInventoryClient>($"{root}/service/"),
            //     "Service",
            //     1000,
            //     false,
            //     false);
            //
            // await DecrementInventory(RestService.For<IInventoryClient>($"{root}/service/"),
            //     "Service",
            //     1000,
            //     false,
            //     true);

            await DecrementInventory(RestService.For<IInventoryClient>($"{root}/grain/persistence/"),
                "Grain Persistence",
                1000,
                false,
                false);
            
            // await DecrementInventory(RestService.For<IInventoryClient>($"{root}/grain/persistence/"),
            //     "Grain Persistence",
            //     1000,
            //     true,
            //     false);
        }

        static async Task DecrementInventory(IInventoryClient client, string name, int max, bool doSynchronously, bool logEachRequest)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var inventoryCheck = new Dictionary<Guid, (decimal expected, decimal actual)>();
            
            for (var index = 0; index < StaticData.Items.Length; index++)
            {
                var item = StaticData.Items[index];
                var newInventory = await client.IncrementInventoryAsync(item.Id, 1000, index);
                inventoryCheck.Add(newInventory.ItemId.Value, (newInventory.Count - max, 0));
            }

            var items = Enumerable.Range(1, max)
                .SelectMany(_ => StaticData.Items)
                .ToArray();
            
            var durations = new List<(TimeSpan duration, Inventory inventory)>();

            if (doSynchronously)
            {
                for (var index = 0; index < items.Length; index++)
                {
                    var item = items[index];
                    var duration = await DecrementInventoryApiCall(client, item, index, logEachRequest);
                    durations.Add(duration);
                }
            }
            else
            {
                var tasks = items
                    .Select((item, index) => DecrementInventoryApiCall(client, item, index, logEachRequest))
                    .ToArray();
                
                await Task.WhenAll(tasks);
                durations.AddRange(tasks.Select(x => x.Result));
            }
            
            stopWatch.Stop();
            
            var avgDuration = TimeSpan.FromMilliseconds(durations.Average(x => x.duration.TotalMilliseconds));
            var slowestDuration = TimeSpan.FromMilliseconds(durations.Max(x => x.duration.TotalMilliseconds));
            var fastestDuration = TimeSpan.FromMilliseconds(durations.Min(x => x.duration.TotalMilliseconds));
            
            System.Console.WriteLine($@"~*~*~*~*~*~* {name} {(doSynchronously ? "Sync" : "Async")}
Total Time {stopWatch.Elapsed}
Avg Request Duration {avgDuration}
Slowest Duration {slowestDuration}
Fastest Duration {fastestDuration}
Total Request Sent {durations.Count}
 ~*~*~*~*~*~*~*~*");

            foreach (var item in StaticData.Items)
            {
                var inv = await client.IncrementInventoryAsync(item.Id, 0, 0);
                var valueTuple = inventoryCheck[item.Id];
                valueTuple.actual = inv.Count;
                inventoryCheck[item.Id] = valueTuple;
            }

            System.Console.WriteLine(Environment.NewLine);

            var invalidInventories = inventoryCheck
                .Where(x => x.Value.actual != x.Value.expected)
                .ToArray();

            if (invalidInventories.Any())
            {
                foreach (var invalidInventory in invalidInventories)
                {
                    System.Console.WriteLine($"!!!! Inventory is incorrect for {invalidInventory.Key}");
                }
            }
            else
            {
                System.Console.WriteLine("No invalid inventories.");
            }

            System.Console.WriteLine(Environment.NewLine);
        }

        private static async Task<(TimeSpan duration, Inventory inventory)> DecrementInventoryApiCall(IInventoryClient client, Item item, int index, bool log)
        {
            var sw = new Stopwatch();
            sw.Start();
            var inv = await client.DecrementInventoryAsync(item.Id, 1, index);
            sw.Stop();
            
            if (log)
            {
                System.Console.WriteLine(
                    $"*************** Index: {index}. Duration: {sw.ElapsedMilliseconds} ms. Item: {inv.Item.Name}. Quantity {inv.Count} **************");
            }

            return (sw.Elapsed, inv);
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