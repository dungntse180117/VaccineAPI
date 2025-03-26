using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class FeedbackResponse
    {
        public int FeedbackId { get; set; }   
        public int VaccinationHistoryId { get; set; }
        public int AccountId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime FeedbackDate { get; set; }
    }

}
