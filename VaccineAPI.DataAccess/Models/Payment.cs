using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int RegistrationId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }

    public string? Status { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Registration Registration { get; set; } = null!;
}
