using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class CreateRegistrationDetailRequest
    {
        [Required]
        public int RegistrationID { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public DateTime? DesiredDate { get; set; }

        public string? Status { get; set; } 
    }

    public class UpdateRegistrationDetailRequest
    {
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime? DesiredDate { get; set; }
        public string? Status { get; set; } 
    }
}