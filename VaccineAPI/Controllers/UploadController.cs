using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;  // Corrected typo
using CloudinaryDotNet.Actions;   // Import ImageUploadResult
using System.ComponentModel.DataAnnotations;

namespace VaccineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly ICloudService _cloudService;
        private readonly IImageService _imageService;
        private readonly ILogger<UploadImageController> _logger;

        public UploadImageController(ICloudService cloudService, IImageService imageService, ILogger<UploadImageController> logger)
        {
            _cloudService = cloudService ?? throw new ArgumentNullException(nameof(cloudService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("UploadBanner")]
        public async Task<IActionResult> UploadBanner([FromForm] UploadRequest req, [FromQuery] int accountId)  //Query because the bannerId is not from model
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state invalid");
                return BadRequest(ModelState);
            }

            if (req == null || req.file == null)
            {
                _logger.LogError("Invalid upload request: File or FileName is missing.");
                return BadRequest("File and FileName are required.");
            }
            if (req.file.Length > 10485760) // limit to 10MB
            {
                _logger.LogError("File size exceeds the limit");
                return BadRequest("File size exceeds the limit");
            }

            try
            {
                // Upload image to cloud storage
                var uploadResult = await _cloudService.UploadImageAsync(req.file);

                if (uploadResult != null && uploadResult.Error == null)
                {
                    // Access the secure URL of the uploaded image
                    string imageUrl = uploadResult.SecureUrl.ToString();
                    _logger.LogInformation($"Uploaded to Cloudinary to: {imageUrl}");

                    // 2. Save the image URL in the database
                    var saveResult = await _imageService.SaveImagesAsync(imageUrl);

                    if (saveResult)
                    {
                        _logger.LogInformation($"Image URL saved to database successfully: {imageUrl}");
                        return Ok("Banner created successfully.");
                    }
                    else
                    {
                        _logger.LogError($"Error saving image URL to database: {imageUrl}");
                        return StatusCode(500, "Error saving image URL to the database. Please try again.");
                    }
                }
                else
                {
                    _logger.LogError($"Upload file error: {uploadResult?.Error?.Message}");
                    return BadRequest("Upload file error: " + uploadResult?.Error?.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file and saving banner information.");
                return StatusCode(500, "Error uploading file and saving banner information.  Please try again."); // Return 500 Internal Server Error
            }
        }

        private bool IsValidFileExtension(string fileExtension)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            return allowedExtensions.Contains(fileExtension.ToLower());
        }
    }
}