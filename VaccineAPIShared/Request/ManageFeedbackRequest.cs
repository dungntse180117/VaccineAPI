using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class ManageFeedbackRequest
    {
        [Required]
        public int FeedbackId { get; set; } // Required for Update and Delete

        public int? AppointmentId { get; set; }
        public int? ServiceId { get; set; }
        public string? Comment { get; set; }
    }

    //Combined Response Model
    public class ManageFeedbackResponse
    {
        public int FeedbackId { get; set; }
        public int? AppointmentId { get; set; }
        public int? ServiceId { get; set; }
        public string? Comment { get; set; }
        public DateTime? FeedbackDate { get; set; }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
