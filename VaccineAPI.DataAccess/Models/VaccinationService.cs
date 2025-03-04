using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationService
{
    public int? DiseaseId { get; set; }

    public string? Description { get; set; }

    public int ServiceId { get; set; }

    public int? VaccinationId { get; set; }

    public int? CategoryId { get; set; }

    public string? ServiceName { get; set; }

    public decimal Price { get; set; }

    public int? AppointmentId { get; set; }

    public virtual VaccinationAppointment? Appointment { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Vaccination? Vaccination { get; set; }
}
