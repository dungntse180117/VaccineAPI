﻿using System;
using System.ComponentModel.DataAnnotations;
namespace VaccineAPI.DataAccess.Models;


public class ChildRequest
{
    [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
    [DataType(DataType.Date)]
    public DateOnly Dob { get; set; }

    [Required(ErrorMessage = "Tên trẻ em là bắt buộc.")]
    [MaxLength(255, ErrorMessage = "Tên trẻ em không được vượt quá 255 ký tự.")]
    public string ChildName { get; set; }

    [MaxLength(10, ErrorMessage = "Giới tính không được vượt quá 10 ký tự.")]
    public string Gender { get; set; }

    [MaxLength(255, ErrorMessage = "Trạng thái tiêm chủng không được vượt quá 255 ký tự.")]
    public string VaccinationStatus { get; set; }
}

