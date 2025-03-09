using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class DiseaseResponse
    {
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; }
        public string Description { get; set; }
    }
}
