using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Disease
{
    public int DiseaseId { get; set; }

    public string DiseaseName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<VaccinationDisease> VaccinationDiseases { get; set; } = new List<VaccinationDisease>();
}
