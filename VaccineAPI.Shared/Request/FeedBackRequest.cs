using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class FeedbackRequest
    {
        public int VaccinationHistoryId { get; set; }  
        public int AccountId { get; set; }   
        public string? Comment { get; set; }  
        public int Rating { get; set; }
    }

}
