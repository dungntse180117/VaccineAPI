using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class PatientResponse
    {
        public int PatientId { get; set; }
        public DateOnly Dob { get; set; }
        public string? PatientName { get; set; }
        public string? Gender { get; set; }
        public string? GuardianPhone { get; set; }
        public string? Address { get; set; }
        public string? RelationshipToAccount { get; set; }
        public string? Phone { get; set; }
    }
}
