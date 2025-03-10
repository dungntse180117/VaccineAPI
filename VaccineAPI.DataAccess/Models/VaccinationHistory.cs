using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationHistory
{
    public int VaccinationHistoryId { get; set; }

    public int AppointmentId { get; set; }

    public DateOnly VaccinationDate { get; set; }

    public string? Reaction { get; set; }

    public int VaccineId { get; set; }

    public string? Notes { get; set; }

    public virtual VaccinationAppointment Appointment { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Vaccination Vaccine { get; set; } = null!;
}
