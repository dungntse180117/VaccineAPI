using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class Image
{
    public int ImgId { get; set; }

    public string Img { get; set; } = null!;

    public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
}
