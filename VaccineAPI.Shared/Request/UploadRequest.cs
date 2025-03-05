using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VaccineAPI.Shared.Request
{
    public class UploadRequest
    {
        [Required(ErrorMessage = "FileName is required")]
        public string FileName { get; set; } = null!; // Nullable string

        [Required(ErrorMessage = "File is required")]
        public IFormFile file { get; set; } = null!; // Nullable for error checking
    }
}