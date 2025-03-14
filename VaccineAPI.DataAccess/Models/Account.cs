using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Banner> Banners { get; set; } = new List<Banner>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<ParentChild> ParentChildren { get; set; } = new List<ParentChild>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<TrafficLog> TrafficLogs { get; set; } = new List<TrafficLog>();

    public virtual ICollection<VaccinationImage> VaccinationImages { get; set; } = new List<VaccinationImage>();
}
