using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class RegistrationDetail
{
    public int RegistrationDetailId { get; set; }

    public int RegistrationId { get; set; }

    public int Quantity { get; set; }

    public DateTime? DesiredDate { get; set; }

    public decimal Price { get; set; }

    public int? PatientId { get; set; }

    public string? Status { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Registration Registration { get; set; } = null!;
}
