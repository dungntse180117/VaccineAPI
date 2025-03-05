using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class ParentChild
{
    public int ParentPatientId { get; set; }

    public int AccountId { get; set; }

    public int PatientId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
