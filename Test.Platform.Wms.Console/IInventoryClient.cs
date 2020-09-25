using System;
using System.Threading.Tasks;
using Refit;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Console
{
    public interface IInventoryClient
    {
        [Post("/increment/{itemId}/{quantity}/{index}")]
        Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index);
        
        [Post("/decrement/{itemId}/{quantity}/{index}")]
        Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index);
    }
}