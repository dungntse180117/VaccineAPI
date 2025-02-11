using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class OrderVaccine
{
    public int OrderId { get; set; }

    public int? VaccinationId { get; set; }

    public decimal? Price { get; set; }

    public int Quantity { get; set; }

    public virtual Vaccination? Vaccination { get; set; }
}
