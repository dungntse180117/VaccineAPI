using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class VisitDayChangeRequestResponse
    {
        public int ChangeRequestId { get; set; }
        public int VisitID { get; set; }
        public int PatientId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string StaffNotes { get; set; }
        public DateTime RequestedDateAt { get; set; }
    }
}
