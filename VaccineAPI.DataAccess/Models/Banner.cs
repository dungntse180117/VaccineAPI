using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Banner
{
    public int BannerId { get; set; }

    public string BannerName { get; set; } = null!;

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
