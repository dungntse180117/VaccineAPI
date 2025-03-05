using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace VaccineAPI.BusinessLogic.Interface
{
    public interface ICloudService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);

        Task<List<ImageUploadResult>> UploadImagesAsync(List<IFormFile> files);
    }
}
