using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class CreateRegistrationRequest
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public List<int> PatientIds { get; set; }

        public List<int> VaccinationIds { get; set; } = new List<int>();

        public int? ServiceId { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public string? Status { get; set; }

        public DateTime? DesiredDate { get; set; }
    }
}