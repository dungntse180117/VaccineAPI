using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Disease
{
    public int DiseaseId { get; set; }

    public string DiseaseName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<VaccinationService> VaccinationServices { get; set; } = new List<VaccinationService>();

    public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
}
