using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Child
{
    public int ChildId { get; set; }

    public DateTime Dob { get; set; }

    public string? ChildName { get; set; }

    public string? Gender { get; set; }

    public string? VaccinationStatus { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<VaccinationAppointment> VaccinationAppointments { get; set; } = new List<VaccinationAppointment>();

    public virtual ICollection<VaccinationHistory> VaccinationHistories { get; set; } = new List<VaccinationHistory>();
}
