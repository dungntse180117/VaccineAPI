using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class VaccinationResponse
    {
        public int VaccinationId { get; set; }
        public string VaccinationName { get; set; } = null!;
        public string? Manufacturer { get; set; }
        public int? TotalDoses { get; set; }
        public int? Interval { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? AgeUnitId { get; set; }
        public int? UnitId { get; set; }
    }
}
