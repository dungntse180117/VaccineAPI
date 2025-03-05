using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class ChildResponse
    {
        public int ChildId { get; set; }
        public DateOnly Dob { get; set; }
        public string ChildName { get; set; }
        public string Gender { get; set; }
        public string VaccinationStatus { get; set; }
    }
}
