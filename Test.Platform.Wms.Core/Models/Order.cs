using System;
using System.Collections.Generic;
using Test.Platform.Wms.Core.Interfaces;

namespace Test.Platform.Wms.Core.Models
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }
        
        public string OrderNumber { get; set; }
        
        public DateTimeOffset CreateDate { get; set; }
        
        public List<OrderLine> Lines { get; set; }
        
        public Address BillTo { get; set; }
        
        public Address ShipTo { get; set; }
    }
}