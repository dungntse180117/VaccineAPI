using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class IntervalUnit
{
    public int UnitId { get; set; }

    public string UnitName { get; set; } = null!;

    public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
}
