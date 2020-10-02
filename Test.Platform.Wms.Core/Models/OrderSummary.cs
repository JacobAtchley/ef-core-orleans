using System;
using System.Collections.Generic;

namespace Test.Platform.Wms.Core.Models
{
    public class OrderSummary
    {
        public Guid Id { get; set; }
        
        public IEnumerable<ItemCount> ItemCounts { get; set; }
    }
}