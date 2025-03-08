using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class VaccinationServiceResponse
    {
        public int serviceID { get; set; }
        public string serviceName { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public int totalDoses { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }

        public List<VaccinationInfo> Vaccinations { get; set; } = new List<VaccinationInfo>();
    }

    public class VaccinationInfo
    {
        public int VaccinationId { get; set; }
        public string VaccinationName { get; set; }
        public List<string> Diseases { get; set; } = new List<string>();
    }
}
