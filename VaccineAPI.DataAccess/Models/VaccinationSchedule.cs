using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationSchedule
{
    public int ScheduleId { get; set; }

    public int VaccinationId { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public string? Dosage { get; set; }

    public int? Interval { get; set; }

    public string? IntervalUnit { get; set; }

    public int? NumberOfDoses { get; set; }

    public string? AgeUnit { get; set; }

    public virtual Vaccination Vaccination { get; set; } = null!;
}
