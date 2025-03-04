using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VaccineAPI.DataAccess.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public IFormFile file { get; set; }
    }
}
