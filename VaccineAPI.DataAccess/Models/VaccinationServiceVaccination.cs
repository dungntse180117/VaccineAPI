using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationServiceVaccination
{
    public int VaccinationServiceVaccinationId { get; set; }

    public int? VaccinationId { get; set; }

    public int? ServiceId { get; set; }

    public virtual VaccinationService? Service { get; set; }

    public virtual Vaccination? Vaccination { get; set; }
}
