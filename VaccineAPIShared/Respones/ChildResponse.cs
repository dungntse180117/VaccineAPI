﻿

namespace VaccineAPI.DataAccess.Models;


public class ChildResponse
{
    public int ChildId { get; set; }
    public DateTime Dob { get; set; }
    public string ChildName { get; set; }
    public string Gender { get; set; }
    public string VaccinationStatus { get; set; }
}
