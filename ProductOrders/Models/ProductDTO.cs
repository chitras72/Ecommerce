using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductOrders.Models
{
    public class ProductDTO
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        [Display(Name="Stock Count")]
        public Nullable<int> UnitInStock { get; set; }
    }
}