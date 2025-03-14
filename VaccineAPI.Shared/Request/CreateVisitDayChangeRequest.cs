using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class CreateVisitDayChangeRequest
    {
        public int VisitID { get; set; }
        public int PatientId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Reason { get; set; }
    }

    public class UpdateVisitDayChangeRequest
    {
        public string Status { get; set; } // "Pending", "Approved", "Rejected"
        public string StaffNotes { get; set; }
    }
}
