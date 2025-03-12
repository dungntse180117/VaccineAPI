using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int AppointmentId { get; set; }

    public DateTime? VisitDate { get; set; }

    public string? Notes { get; set; }

    public string? Status { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual ICollection<VaccinationHistory> VaccinationHistories { get; set; } = new List<VaccinationHistory>();

    public virtual ICollection<VisitVaccination> VisitVaccinations { get; set; } = new List<VisitVaccination>();
}
