using System;
using System.ComponentModel.DataAnnotations;
namespace VaccineAPI.DataAccess.Models;


public class ChildRequest
{
    [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
    [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/\d{4}$", ErrorMessage = "Vui lòng nhập ngày sinh theo định dạng dd/MM/yyyy")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime Dob { get; set; }

    [Required(ErrorMessage = "Tên trẻ em là bắt buộc.")]
    [MaxLength(255, ErrorMessage = "Tên trẻ em không được vượt quá 255 ký tự.")]
    public string ChildName { get; set; }

    [MaxLength(10, ErrorMessage = "Giới tính không được vượt quá 10 ký tự.")]
    public string Gender { get; set; }

    [MaxLength(255, ErrorMessage = "Trạng thái tiêm chủng không được vượt quá 255 ký tự.")]
    public string VaccinationStatus { get; set; }
}
