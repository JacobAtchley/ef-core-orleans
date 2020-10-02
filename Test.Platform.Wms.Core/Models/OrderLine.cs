using System;

namespace Test.Platform.Wms.Core.Models
{
    public class OrderLine
    {
        public Guid Id { get; set; }
        
        public Guid ItemId { get; set; }
        
        public virtual OrderLineItem Item { get; set; }
        
        public decimal Quantity { get; set; }
    }

    public class OrderLineItem
    {
        public Guid ItemId { get; set; }
        
        public string ItemName { get; set; }
        
        public string ItemDescription { get; set; }
    }
}