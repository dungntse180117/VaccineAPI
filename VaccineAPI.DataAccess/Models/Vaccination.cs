using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Vaccination
{
    public int VaccinationId { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public string? Img { get; set; }

    public string VaccinationName { get; set; } = null!;

    public virtual ICollection<OrderVaccine> OrderVaccines { get; set; } = new List<OrderVaccine>();

    public virtual ICollection<VaccinationService> VaccinationServices { get; set; } = new List<VaccinationService>();
}
