using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Implement
{
    public class ImageService : IImageService
    {
        private readonly VaccinationTrackingContext _context;
        private readonly string _imageBasePath;
        private readonly ICloudService _cloudService; //Inject Cloud Service

        public ImageService(VaccinationTrackingContext context, IConfiguration configuration, ICloudService cloudService)
        {
            _context = context;
            _imageBasePath = configuration["ImageStorage:BasePath"];
            _cloudService = cloudService; //Init Cloud Service
        }

        public async Task<List<ImageResponse>> GetAllImagesAsync()
        {
            var images = await _context.Images.ToListAsync();
            return images.Select(MapToImageResponse).ToList();
        }

        public async Task<ImageResponse> GetImageByIdAsync(int imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
            return image == null ? null : MapToImageResponse(image);
        }

        public async Task<ImageResponse> CreateImageAsync(ImageRequest request)
        {
            var image = new Image
            {
                Img = request.Img
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return MapToImageResponse(image);
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
            if (image == null)
            {
                return false;
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_imageBasePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }

        private ImageResponse MapToImageResponse(Image image)
        {
            return new ImageResponse
            {
                ImageId = image.ImgId,
                Img = image.Img
            };
        }
        //public async Task<bool> SaveImagesAsync(string imageUrl) // Removed Old
        //{
        //    if (string.IsNullOrEmpty(imageUrl))
        //    {
        //        return false;
        //    }

        //    try
        //    {
        //        var image = new Image { Img = imageUrl };
        //        _context.Images.Add(image);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error saving image: {ex.Message}");
        //        return false;
        //    }
        //}

        public async Task<ImageResponse> SaveVaccinationImageAsync(IFormFile file, int vaccinationId, int accountId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            // 1. Upload image to cloud storage
            var uploadResult = await _cloudService.UploadImageAsync(file);

            if (uploadResult == null)
            {
                throw new Exception("Failed to upload image to Cloudinary.");
            }

            // 2. Save image information to the Image table
            var image = new Image { Img = uploadResult.SecureUrl.ToString() };
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            // 3. Create a record in the Vaccination_Image table
            var vaccinationImage = new VaccinationImage
            {
                ImgId = image.ImgId,
                VaccinationId = vaccinationId,
                AccountId = accountId
            };
            _context.VaccinationImages.Add(vaccinationImage);
            await _context.SaveChangesAsync();

            return MapToImageResponse(image);
        }
    }
}