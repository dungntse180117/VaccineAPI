using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class RegistrationVaccination
{
    public int RegistrationPatientId { get; set; }

    public int RegistrationId { get; set; }

    public int? VaccinationId { get; set; }

    public virtual Registration Registration { get; set; } = null!;

    public virtual Vaccination? Vaccination { get; set; }
}
