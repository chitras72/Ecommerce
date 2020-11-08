using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductOrders.Models
{
    public class Error
    {
        public int StatusCode { get; set; }
        public String ErrorMessage { get; set; }
    }
}