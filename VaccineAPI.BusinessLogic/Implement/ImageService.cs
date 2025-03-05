using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VaccineAPI.BusinessLogic.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;

namespace VaccineAPI.BusinessLogic.Implement
{
    public class ImageService : IImageService
    {
        private readonly VaccinationTrackingContext _context;
        private readonly ILogger<ImageService> _logger;

        public ImageService(VaccinationTrackingContext context, ILogger<ImageService> logger) //Add logger
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SaveImagesAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentException("Image URL cannot be null or empty.", nameof(imageUrl));
            }
            try
            {
                var banner = new Banner
                {
                    BannerName = imageUrl,
                    //AccountId = you will need to link the account later but i have it nullable right now
                };

                _context.Banners.Add(banner);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Image URL saved to the database: {imageUrl}");// Log

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving image URL to the database: {imageUrl}");// Log Error
                return false;
            }
        }
    }
}

