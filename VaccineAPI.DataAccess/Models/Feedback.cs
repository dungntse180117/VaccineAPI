using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? AppointmentId { get; set; }

    public int? ServiceId { get; set; }

    public string? Comment { get; set; }

    public DateOnly? FeedbackDate { get; set; }

    public virtual VaccinationAppointment? Appointment { get; set; }

    public virtual VaccinationService? Service { get; set; }
}
