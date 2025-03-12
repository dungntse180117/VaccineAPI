﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class AppointmentResponse
    {
        public int AppointmentID { get; set; }
        public int RegistrationDetailID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int? ConfigId { get; set; }
        public int? AppointmentNumber { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public int PatientId { get; set; }
        public String? PatientName { get; set; }
    }
}
