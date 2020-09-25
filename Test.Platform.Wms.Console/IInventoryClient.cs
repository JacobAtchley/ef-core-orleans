using System;
using System.Threading.Tasks;
using Refit;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Console
{
    public interface IInventoryClient
    {
        [Post("/increment/{itemId}/{quantity}")]
        Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity);
        
        [Post("/decrement/{itemId}/{quantity}")]
        Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity);
    }
}