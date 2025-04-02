using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? AccountId { get; set; }

    public string? Comment { get; set; }

    public int? Rating { get; set; }

    public string? Status { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public int? VisitId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Visit? Visit { get; set; }
}
