using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationHistory
{
    public int VaccinationHistoryId { get; set; }

    public int VisitId { get; set; }

    public DateTime VaccinationDate { get; set; }

    public string? Reaction { get; set; }

    public int VaccineId { get; set; }

    public string? Notes { get; set; }

    public int? PatientId { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Patient? Patient { get; set; }

    public virtual Vaccination Vaccine { get; set; } = null!;

    public virtual Visit Visit { get; set; } = null!;
}
