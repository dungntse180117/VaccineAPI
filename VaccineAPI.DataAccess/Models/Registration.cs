using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public int AccountId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }

    public int? ServiceId { get; set; }

    public DateTime? DesiredDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<RegistrationPatient> RegistrationPatients { get; set; } = new List<RegistrationPatient>();

    public virtual ICollection<RegistrationVaccination> RegistrationVaccinations { get; set; } = new List<RegistrationVaccination>();

    public virtual VaccinationService? Service { get; set; }

    public virtual ICollection<VaccinationAppointment> VaccinationAppointments { get; set; } = new List<VaccinationAppointment>();
}
