using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class VaccinationHistoryResponse
    {
        public int VaccinationHistoryID { get; set; }
        public int VisitID { get; set; }
        public DateTime VaccinationDate { get; set; }
        public string Reaction { get; set; }
        public int VaccineId { get; set; }
        public string Notes { get; set; }
        public int? PatientId { get; set; }
    }
}
