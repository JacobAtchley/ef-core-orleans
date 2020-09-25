using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Platform.Wms.Core.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
    }
}