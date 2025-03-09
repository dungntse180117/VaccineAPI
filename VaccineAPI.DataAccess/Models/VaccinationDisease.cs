using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationDisease
{
    public int VaccinationDiseId { get; set; }

    public int VaccinationId { get; set; }

    public int DiseaseId { get; set; }

    public virtual Disease Disease { get; set; } = null!;

    public virtual Vaccination Vaccination { get; set; } = null!;
}
