using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class FeedbackRequest
    {
    }
    public class CreateFeedbackRequest
    {
        public int? AccountId { get; set; } 
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int VisitId { get; set; }
    }

    public class UpdateFeedbackRequest
    {
        public int FeedbackId { get; set; }
        public string? Comment { get; set; }
        public int? Rating { get; set; }      
        public string? Status { get; set; }   
    }

}
