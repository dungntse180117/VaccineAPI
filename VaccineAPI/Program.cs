using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.Services;
using VaccineAPI.Shared;
using Microsoft.OpenApi.Models;
using VaccineAPI.Shared.Helpers;
using VaccineAPI.BusinessLogic.Services.Implement;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Authorization;
using VaccineAPI.BusinessLogic.Implement;
using VaccineAPI.BusinessLogic.Interface;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.BusinessLogic.Services;
//using IVaccineOrderService = VaccineAPI.BusinessLogic.Implement.IVaccineOrderService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
//builder.Services.AddScoped<IVaccineOrderService, VaccineOrderService>(); // Register IVaccineOrderService
//builder.Services.AddScoped<IChildService, ChildService>();
//builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPatientService, PatientService>();
// Configure DbContext
builder.Services.AddDbContext<VaccinationTrackingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"]!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Add JWT to swagger 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vaccine API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\nEnter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
});

// Add services
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDiseaseService, DiseaseService>();
builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **Use CORS**
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<TrafficLoggingMiddleware>();

app.MapControllers();

app.Run();
