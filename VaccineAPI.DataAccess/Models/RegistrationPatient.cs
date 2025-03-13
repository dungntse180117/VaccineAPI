using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class RegistrationPatient
{
    public int RegistrationPatientId { get; set; }

    public int RegistrationId { get; set; }

    public int PatientId { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Registration Registration { get; set; } = null!;
}
