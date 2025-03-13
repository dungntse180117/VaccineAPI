using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VisitDayChangeRequest
{
    public int ChangeRequestId { get; set; }

    public int VisitId { get; set; }

    public int PatientId { get; set; }

    public DateTime RequestedDate { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public string? StaffNotes { get; set; }

    public DateTime RequestedDateAt { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Visit Visit { get; set; } = null!;
}
