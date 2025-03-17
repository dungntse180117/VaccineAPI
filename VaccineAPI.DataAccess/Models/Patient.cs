using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public DateOnly Dob { get; set; }

    public string? PatientName { get; set; }

    public string? Gender { get; set; }

    public string? GuardianPhone { get; set; }

    public string? Address { get; set; }

    public string? RelationshipToAccount { get; set; }

    public string? Phone { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<RegistrationDetail> RegistrationDetails { get; set; } = new List<RegistrationDetail>();

    public virtual ICollection<RegistrationPatient> RegistrationPatients { get; set; } = new List<RegistrationPatient>();

    public virtual ICollection<VaccinationHistory> VaccinationHistories { get; set; } = new List<VaccinationHistory>();

    public virtual ICollection<VisitDayChangeRequest> VisitDayChangeRequests { get; set; } = new List<VisitDayChangeRequest>();
}
