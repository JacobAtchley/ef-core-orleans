using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Platform.Wms.Core.Models
{
    public class Inventory
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid? ItemId { get; set; }
        
        public virtual Item Item { get; set; }
        
        [Required]
        public decimal Count { get; set; }
    }
}