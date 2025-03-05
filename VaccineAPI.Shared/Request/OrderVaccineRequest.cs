using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class OrderVaccineRequest
    {
        public int VaccinationId { get; set; }
        public int Quantity { get; set; }
    }
}