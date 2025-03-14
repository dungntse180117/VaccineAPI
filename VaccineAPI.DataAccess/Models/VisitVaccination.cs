using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VisitVaccination
{
    public int AppointmentDoseVaccinationId { get; set; }

    public int VisitId { get; set; }

    public int AppointmentVaccinationId { get; set; }

    public virtual AppointmentVaccination AppointmentVaccination { get; set; } = null!;

    public virtual Visit Visit { get; set; } = null!;
}
