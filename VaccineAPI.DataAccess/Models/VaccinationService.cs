using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationService
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public int? CategoryId { get; set; }

    public int? TotalDoses { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<VaccinationServiceVaccination> VaccinationServiceVaccinations { get; set; } = new List<VaccinationServiceVaccination>();
}
