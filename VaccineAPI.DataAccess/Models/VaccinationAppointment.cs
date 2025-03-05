using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationAppointment
{
    public int AppointmentId { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public int AccountId { get; set; }

    public int? PatientId { get; set; }

    public int? ConfigId { get; set; }

    public int? AppointmentNumber { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

    public virtual Configuration? Config { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Patient? Patient { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
