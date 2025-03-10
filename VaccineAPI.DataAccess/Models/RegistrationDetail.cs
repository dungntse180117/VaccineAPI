using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class RegistrationDetail
{
    public int RegistrationDetailId { get; set; }

    public int RegistrationPatientId { get; set; }

    public int Quantity { get; set; }

    public DateTime? DesiredDate { get; set; }

    public decimal Price { get; set; }

    public virtual RegistrationPatient RegistrationPatient { get; set; } = null!;
}
