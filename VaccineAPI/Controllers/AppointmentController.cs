using Microsoft.AspNetCore.Mvc;
using VaccineAPI.BusinessLogic.Services.Implement;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;


        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentDetail(int id)
        {
            var appointment = await _appointmentService.GetAppointmentDetailsAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointment()
        {
            var appointments = await _appointmentService.GetAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                var appointment = await _appointmentService.CreateAppointmentAsync(request);
                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentID }, appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentRequest request)
        {
            try
            {
                await _appointmentService.UpdateAppointmentAsync(id, request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                await _appointmentService.DeleteAppointmentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("{appointmentId}/vaccinations")]
        public async Task<IActionResult> GetAppointmentVaccinations(int appointmentId)
        {
            var appointmentVaccinations = await _appointmentService.GetAppointmentVaccinationsAsync(appointmentId);
            return Ok(appointmentVaccinations);
        }
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetRegistrationsWithAppointmentsByAccountId(int accountId)
        {
            var registrations = await _appointmentService.GetRegistrationsWithAppointmentsByAccountIdAsync(accountId);
            return Ok(registrations);
        } 
    }
}