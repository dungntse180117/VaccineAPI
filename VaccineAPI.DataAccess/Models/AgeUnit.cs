using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class AgeUnit
{
    public int AgeUnitId { get; set; }

    public string AgeUnitName { get; set; } = null!;

    public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
}
