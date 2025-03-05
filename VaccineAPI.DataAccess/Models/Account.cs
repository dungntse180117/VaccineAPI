using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace VaccineAPI.DataAccess.Models;

public partial class Account
{
    private string cloudName;
    private string apiKey;
    private string apiSecret;

    public string account {  get; set; }

    public int AccountId { get; set; }

    public string Name { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int? ChildId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Banner> Banners { get; set; } = new List<Banner>();

    public virtual Child? Child { get; set; }

    public virtual Role Role { get; set; } = null!;
}
