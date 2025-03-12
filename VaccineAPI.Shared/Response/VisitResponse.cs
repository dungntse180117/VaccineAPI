using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;

namespace VaccineAPI.Shared.Response
{
    public class VisitResponse
    {
        public int VisitID { get; set; }
        public int AppointmentID { get; set; }
        public DateTime? VisitDate { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        public List<VisitVaccinationInfo> VisitVaccinations { get; set; }
    }
}
