﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class VaccinationServiceVaccinationRequest
    {
        [Required]
        public int ServiceID { get; set; }
        [Required]
        public int VaccinationID { get; set; }
    }
}
