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

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<VaccinationHistory> VaccinationHistories { get; set; } = new List<VaccinationHistory>();

    public virtual ICollection<VisitDayChangeRequest> VisitDayChangeRequests { get; set; } = new List<VisitDayChangeRequest>();

    public virtual ICollection<VisitVaccination> VisitVaccinations { get; set; } = new List<VisitVaccination>();
}
