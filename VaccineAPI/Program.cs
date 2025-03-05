using Microsoft.EntityFrameworkCore;
using VaccineAPI.BusinessLogic.Implement;
using VaccineAPI.BusinessLogic.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Helpers;
using IVaccineOrderService = VaccineAPI.BusinessLogic.Implement.IVaccineOrderService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<VaccinationTrackingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IVaccineOrderService, VaccineOrderService>(); // Register IVaccineOrderService

// Register other necessary services
builder.Services.AddScoped<IChildService, ChildService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Configure logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
