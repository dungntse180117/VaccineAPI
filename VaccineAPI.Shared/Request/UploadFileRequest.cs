using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Request
{
    public class UploadFileRequest
    {
        public IFormFile? File { get; set; }
        public int VaccinationId { get; set; }
        public int AccountId { get; set; }
    }
}
