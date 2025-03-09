using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationService
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public int? CategoryId { get; set; }

    public int? DiseaseId { get; set; }

    public int? VaccinationId { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

    public virtual Category? Category { get; set; }

    public virtual Disease? Disease { get; set; }

    public virtual Vaccination? Vaccination { get; set; }
}
