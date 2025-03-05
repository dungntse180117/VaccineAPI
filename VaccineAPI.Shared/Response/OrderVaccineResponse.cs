using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Respones
{
    public class OrderVaccineResponse
    {
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; } // Added total price
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}