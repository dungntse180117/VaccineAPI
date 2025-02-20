using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class TrafficLog
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? RequestPath { get; set; }

    public string RequestMethod { get; set; } = null!;

    public bool IsRegistered { get; set; }

    public int? AccountId { get; set; }
}
