using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<VaccinationService> VaccinationServices { get; set; } = new List<VaccinationService>();
}
