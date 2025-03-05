using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Vaccination
{
    public int VaccinationId { get; set; }

    public string VaccinationName { get; set; } = null!;

    public string? Manufacturer { get; set; }

    public string? Dose { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public string? Img { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public virtual ICollection<VaccinationHistory> VaccinationHistories { get; set; } = new List<VaccinationHistory>();

    public virtual ICollection<VaccinationSchedule> VaccinationSchedules { get; set; } = new List<VaccinationSchedule>();

    public virtual ICollection<VaccinationService> VaccinationServices { get; set; } = new List<VaccinationService>();

    public virtual ICollection<Disease> Diseases { get; set; } = new List<Disease>();
}
