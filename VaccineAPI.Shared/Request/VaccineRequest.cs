//using System;
//using System.ComponentModel.DataAnnotations;

//namespace VaccineAPI.Shared.Request
//{
//    public class VaccineRequest
//    {
//        public VaccineRequest()
//        {
//            Description = string.Empty;
//            Img = string.Empty;
//            VaccinationName = string.Empty;
//        }

//        [Required(ErrorMessage = "Giá là bắt buộc.")]
//        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0.")]
//        public decimal? Price { get; set; }

//        [MaxLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
//        public string Description { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
//        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0.")]
//        public int Quantity { get; set; }

//        [MaxLength(255, ErrorMessage = "Đường dẫn ảnh không được vượt quá 255 ký tự.")]
//        public string Img { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Tên vaccine là bắt buộc.")]
//        [MaxLength(255, ErrorMessage = "Tên vaccine không được vượt quá 255 ký tự.")]
//        public string VaccinationName { get; set; } = string.Empty;
//    }
//}