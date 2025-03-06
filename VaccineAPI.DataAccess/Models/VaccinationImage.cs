using System;
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public partial class VaccinationImage
{
    public int VaccinationImgId { get; set; }

    public int ImgId { get; set; }

    public int VaccinationId { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Image Img { get; set; } = null!;

    public virtual Vaccination Vaccination { get; set; } = null!;
}
