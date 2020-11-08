using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductOrders.Models
{
    public class OrderDTO
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime Date { get; set; }
    }
}