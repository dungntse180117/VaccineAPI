
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class GetFeedbackResponse
    {
        public int FeedbackId { get; set; }
        public int? AppointmentId { get; set; }
        public int? ServiceId { get; set; }
        public string? Comment { get; set; }
        public DateTime? FeedbackDate { get; set; }
    }

}
