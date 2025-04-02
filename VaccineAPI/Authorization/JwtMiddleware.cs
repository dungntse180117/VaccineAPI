using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Services;
using VaccineAPI.Shared.Helpers;

namespace VaccineAPI.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)  // Only RequestDelegate and ILogger
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAccountService accountService, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings) // Inject dependencies here!
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var accountId = jwtUtils.ValidateJwtToken(token);
                    if (accountId != null)
                    {
                        // Attach user to context on successful jwt validation
                        context.Items["Account"] = await accountService.GetByIdAsync(accountId.Value);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating JWT token");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await _next(context);
        }
    }
}