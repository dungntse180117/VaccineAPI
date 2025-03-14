using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class CreateAppointmentRequest
    {
        public int RegistrationDetailID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int? ConfigId { get; set; }
        public int? AppointmentNumber { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateAppointmentRequest
    {
        public DateTime? AppointmentDate { get; set; }
        public int? ConfigId { get; set; }
        public int? AppointmentNumber { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

}
