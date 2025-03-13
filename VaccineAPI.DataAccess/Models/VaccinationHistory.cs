using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationHistory
{
    public int VaccinationId { get; set; }

    public DateOnly VaccinationDate { get; set; }

    public string? Reaction { get; set; }

    public int VaccineId { get; set; }

    public int? PatientId { get; set; }

    public int? ConfigId { get; set; }

    public string? Notes { get; set; }

    public virtual Configuration? Config { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Vaccination Vaccine { get; set; } = null!;
}
