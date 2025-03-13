using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int RegistrationDetailId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public int? ConfigId { get; set; }

    public int? AppointmentNumber { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<AppointmentVaccination> AppointmentVaccinations { get; set; } = new List<AppointmentVaccination>();

    public virtual Configuration? Config { get; set; }

    public virtual RegistrationDetail RegistrationDetail { get; set; } = null!;

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
