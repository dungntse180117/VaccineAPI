﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class UpdateDiseaseRequest
    {
        public string DiseaseName { get; set; }
        public string Description { get; set; }
    }
}
