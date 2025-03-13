using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class AppointmentDetailsResponse
    {
        public int AppointmentID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public List<AppointmentVaccinationDetails> AppointmentVaccinations { get; set; }
    }

}
