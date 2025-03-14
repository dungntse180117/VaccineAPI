using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Vaccination
{
    public int VaccinationId { get; set; }

    public string VaccinationName { get; set; } = null!;

    public string? Manufacturer { get; set; }

    public int? TotalDoses { get; set; }

    public int? Interval { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public int? AgeUnitId { get; set; }

    public int? UnitId { get; set; }

    public virtual AgeUnit? AgeUnit { get; set; }

    public virtual ICollection<AppointmentVaccination> AppointmentVaccinations { get; set; } = new List<AppointmentVaccination>();

    public virtual ICollection<RegistrationVaccination> RegistrationVaccinations { get; set; } = new List<RegistrationVaccination>();

    public virtual IntervalUnit? Unit { get; set; }

    public virtual ICollection<VaccinationDisease> VaccinationDiseases { get; set; } = new List<VaccinationDisease>();

    public virtual ICollection<VaccinationHistory> VaccinationHistories { get; set; } = new List<VaccinationHistory>();

    public virtual ICollection<VaccinationImage> VaccinationImages { get; set; } = new List<VaccinationImage>();

    public virtual ICollection<VaccinationServiceVaccination> VaccinationServiceVaccinations { get; set; } = new List<VaccinationServiceVaccination>();
}
