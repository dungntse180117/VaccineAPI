using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class AppointmentService
{
    public int AppointmentServiceId { get; set; }

    public int AppointmentId { get; set; }

    public int ServiceId { get; set; }

    public virtual VaccinationAppointment Appointment { get; set; } = null!;

    public virtual VaccinationService Service { get; set; } = null!;
}
