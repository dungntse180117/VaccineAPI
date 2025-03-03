using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Banner
{
    public int BannerId { get; set; }

    public string? BannerName { get; set; }

    public string? BannerImage { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
