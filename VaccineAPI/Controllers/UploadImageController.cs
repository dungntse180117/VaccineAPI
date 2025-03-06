using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Interface;
using Microsoft.AspNetCore.Http;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;
using System.Net;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost("UploadFile")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ImageResponse>> UploadFile([FromForm] UploadFileRequest req)
        {
            if (req.File == null || req.File.Length == 0)
            {
                _logger.LogError("Invalid upload request: File is missing.");
                return BadRequest("File is required.");
            }

            try
            {
                //Call new service
                var result = await _imageService.SaveVaccinationImageAsync(req.File, req.VaccinationId, req.AccountId);

                if (result != null)
                {
                    _logger.LogInformation($"Image uploaded and associated with vaccination {req.VaccinationId} successfully. URL: {result.Img}");

                    return Ok(result);
                }
                else
                {
                    _logger.LogError("Error saving vaccination image.");
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Error saving vaccination image.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file and saving vaccination image information.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error uploading file and saving vaccination image information.");
            }
        }
    }
}