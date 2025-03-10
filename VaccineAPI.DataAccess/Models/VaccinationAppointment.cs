using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationAppointment
{
    public int AppointmentId { get; set; }

    public int RegistrationId { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public int? ConfigId { get; set; }

    public int? AppointmentNumber { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

    public virtual Configuration? Config { get; set; }

    public virtual Registration Registration { get; set; } = null!;

    public virtual ICollection<VaccinationHistory> VaccinationHistories { get; set; } = new List<VaccinationHistory>();
}
