using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationAppointment
{
    public int AppointmentId { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public int? ServiceId { get; set; }

    public string? Status { get; set; }

    public int? ChildId { get; set; }

    public DateOnly? ScheduledDate { get; set; }

    public virtual Child? Child { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<VaccinationService> VaccinationServices { get; set; } = new List<VaccinationService>();
}
