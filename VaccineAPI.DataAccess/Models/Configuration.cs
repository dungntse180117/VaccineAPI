using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Configuration
{
    public int ConfigId { get; set; }

    public string FacilityName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
