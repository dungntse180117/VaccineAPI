using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class RegistrationWithAppointmentsResponse
    {
        public int RegistrationId { get; set; }
        public int AccountId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime? DesiredDate { get; set; }
        public List<AppointmentDetails> AppointmentDetails { get; set; }
    }

    public class AppointmentDetails
    {
        public int AppointmentID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public List<AppointmentVaccinationDetails> AppointmentVaccinations { get; set; }
    }

    public class AppointmentVaccinationDetails
    {
        public int AppointmentVaccinationID { get; set; }
        public int VaccinationId { get; set; }
        public string VaccinationName { get; set; }
        public int TotalDoses { get; set; }
        public int DosesRemaining { get; set; }
        public string Status { get; set; }
    }
}
