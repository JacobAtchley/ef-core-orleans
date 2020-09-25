using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Services.Interfaces
{
    public interface IIncrementInventoryService
    {
        Task<Inventory> IncrementAsync(Guid itemId, decimal amount, CancellationToken cancellationToken);
    }
}