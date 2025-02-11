using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationHistory
{
    public int VaccinationId { get; set; }

    public DateOnly VaccinationDate { get; set; }

    public string? Reaction { get; set; }

    public int VaccineId { get; set; }

    public int ChildId { get; set; }

    public int FacilityId { get; set; }

    public virtual Child Child { get; set; } = null!;
}
