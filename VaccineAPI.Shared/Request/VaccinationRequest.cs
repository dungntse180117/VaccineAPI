using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class VaccinationRequest
    {

        [Required(ErrorMessage = "Tên vaccine là bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Tên vaccine không được vượt quá 255 ký tự.")]
        public string VaccinationName { get; set; } = null!;

        [MaxLength(255, ErrorMessage = "Nhà sản xuất không được vượt quá 255 ký tự.")]
        public string? Manufacturer { get; set; }

        [Range(1, 10, ErrorMessage = "Số mũi tiêm phải từ 1 đến 10.")]
        public int? TotalDoses { get; set; }

        public int? Interval { get; set; }

        [Range(0, 1000000, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0.")]
        public decimal? Price { get; set; }

        [MaxLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
        public string? Description { get; set; }

        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? AgeUnitId { get; set; }
        public int? UnitId { get; set; }
    }
}
