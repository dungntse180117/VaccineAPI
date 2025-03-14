using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class RegistrationResponse
    {
        public int registrationID { get; set; }
        public int AccountId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public DateTime? DesiredDate { get; set; }
    }
}
