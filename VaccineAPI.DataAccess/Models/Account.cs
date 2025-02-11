using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Name { get; set; } = null!;

    public int RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public int? ChildId { get; set; }

    public virtual Child? Child { get; set; }

    public virtual Role Role { get; set; } = null!;
}
