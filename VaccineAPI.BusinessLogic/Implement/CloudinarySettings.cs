using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VaccineAPI.BusinessLogic.Implement
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ApiSecret { get; set; } = string.Empty;
        public string UploadPreset { get; set; } = string.Empty;
    }

    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<List<ImageUploadResult>> UploadImagesAsync(List<IFormFile> files);
    }

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _uploadPreset;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettingsOptions)
        {
            var cloudinarySettings = cloudinarySettingsOptions.Value;

            Account account = new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret);

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
            _uploadPreset = cloudinarySettings.UploadPreset;
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty.", nameof(file));
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                throw new ArgumentException("File must be an image.", nameof(file));
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        UploadPreset = _uploadPreset,
                    };

                    return await _cloudinary.UploadAsync(uploadParams);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to upload image to Cloudinary.", ex);
            }
        }

        public async Task<List<ImageUploadResult>> UploadImagesAsync(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("File list cannot be null or empty.", nameof(files));
            }

            var uploadResults = new List<ImageUploadResult>();
            foreach (var file in files)
            {
                try
                {
                    var result = await UploadImageAsync(file);
                    uploadResults.Add(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while uploading file: {file.FileName}. Error message: {ex.Message}");
                    // Depending on your need, you can return empty list or send list with data
                }
            }
            return uploadResults;
        }
    }
}