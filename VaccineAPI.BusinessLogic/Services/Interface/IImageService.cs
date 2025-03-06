using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Interface
{
    public interface IImageService
    {
        Task<List<ImageResponse>> GetAllImagesAsync();
        Task<ImageResponse> GetImageByIdAsync(int imageId);
        Task<ImageResponse> CreateImageAsync(ImageRequest request);
        Task<bool> DeleteImageAsync(int imageId);
        Task<string> SaveImageAsync(IFormFile file);
        Task<ImageResponse> SaveVaccinationImageAsync(IFormFile file, int vaccinationId, int accountId);
    }
}
    