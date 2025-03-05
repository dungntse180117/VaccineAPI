    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VaccineAPI.DataAccess.Models;

    namespace VaccineAPI.Shared.Request
    {
        public class CreatePatientRequest
        {
            [Required]
            public DateOnly Dob { get; set; }

            [Required]
            public string? PatientName { get; set; }

            public string? Gender { get; set; }

            public string? GuardianPhone { get; set; }

            public string? Address { get; set; }

            public string? RelationshipToAccount { get; set; }

            public string? Phone { get; set; }

            [Required(ErrorMessage = "AccountId là bắt buộc.")]  
            public int AccountId { get; set; }
        }
    }
