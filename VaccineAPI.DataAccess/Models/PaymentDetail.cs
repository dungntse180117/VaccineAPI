using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class PaymentDetail
{
    public int PaymentDetailId { get; set; }

    public int PaymentId { get; set; }

    public int? TotalPrice { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
