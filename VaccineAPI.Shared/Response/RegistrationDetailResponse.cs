using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class RegistrationDetailResponse
    {
        public int RegistrationDetailID { get; set; }
        public int RegistrationID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime? DesiredDate { get; set; }
        public int PatientId { get; set; }
        public string? Status { get; set; }

        public List<string> VaccinationNames { get; set; } = new List<string>();
        public string? ServiceName { get; set; }
        public int AccountId { get; set; }
    }
}