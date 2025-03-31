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
        public int? AccountId { get; set; } 
        public string? AccountName { get; set; } 
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public string? Status { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public int VisitId { get; set; }
        public DateTime? VisitDate { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
