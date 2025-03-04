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

<<<<<<< HEAD
    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Banner> Banners { get; set; } = new List<Banner>();

=======
>>>>>>> parent of e915893 (Child (date not fix))
    public virtual Child? Child { get; set; }

    public virtual Role Role { get; set; } = null!;
}
