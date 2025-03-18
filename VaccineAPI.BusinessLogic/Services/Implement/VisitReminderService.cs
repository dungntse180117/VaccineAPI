using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;

namespace VaccineAPI.BusinessLogic.Services.Implement
{
    public class VisitReminderService : BackgroundService
    {
        private readonly ILogger<VisitReminderService> _logger;
        private readonly IServiceProvider _serviceProvider; 

        public VisitReminderService(ILogger<VisitReminderService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try {  
                
                    using (var scope = _serviceProvider.CreateScope()) 
                    {
                        var visitService = scope.ServiceProvider.GetRequiredService<IVisitService>(); 
                        await visitService.SendVisitReminderEmailsAsync();
                    }

                    _logger.LogInformation("Đã gửi email nhắc nhở lịch tiêm thành công.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Lỗi khi gửi email nhắc nhở lịch tiêm: {ex.Message}");
                }

             
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}