using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class PaymentStatus
{
    public int PaymentStatusId { get; set; }

    public string StatusName { get; set; } = null!;
}
