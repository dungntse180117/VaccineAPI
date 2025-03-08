using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class VaccinationServiceRequest
    {
        [Required]
        [MaxLength(255)]
        public string ServiceName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        
    }
}
