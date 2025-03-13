using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class CreateVisitRequest
    {
        public int AppointmentID { get; set; }
        public DateTime? VisitDate { get; set; }
        public string Notes { get; set; }
        public List<int> AppointmentVaccinationIds { get; set; } 
    }
    public class VisitVaccinationInfo
    {
        public int AppointmentVaccinationID { get; set; }
        public int VaccinationId { get; set; }
        public string VaccinationName { get; set; }
    }
    public class UpdateVisitStatusRequest
    {
        public string Status { get; set; } 
    }
}
