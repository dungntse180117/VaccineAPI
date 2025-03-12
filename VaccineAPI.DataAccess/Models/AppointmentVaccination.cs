using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class AppointmentVaccination
{
    public int AppointmentVaccinationId { get; set; }

    public int AppointmentId { get; set; }

    public int VaccinationId { get; set; }

    public int TotalDoses { get; set; }

    public int DosesRemaining { get; set; }

    public int DosesScheduled { get; set; }

    public string? VaccinationName { get; set; }

    public string? Status { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Vaccination Vaccination { get; set; } = null!;

    public virtual ICollection<VisitVaccination> VisitVaccinations { get; set; } = new List<VisitVaccination>();
}
