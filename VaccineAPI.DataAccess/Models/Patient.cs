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

    public virtual ICollection<ParentChild> ParentChildren { get; set; } = new List<ParentChild>();

    public virtual ICollection<RegistrationPatient> RegistrationPatients { get; set; } = new List<RegistrationPatient>();
}
