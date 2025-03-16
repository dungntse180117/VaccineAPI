using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                PatientResponse response = await _patientService.CreatePatientAsync(request);
                return CreatedAtAction(nameof(GetPatient), new { id = response.PatientId }, response);
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây (sử dụng _logger)
                return BadRequest(ex.Message); // Hoặc trả về mã lỗi 500 InternalServerError
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            PatientResponse? response = await _patientService.GetPatientByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            List<PatientResponse> responses = await _patientService.GetAllPatientsAsync();

            return Ok(responses);
        }

        [HttpGet("byaccount/{accountId}")]
        public async Task<IActionResult> GetAllPatientsByAccountId(int accountId)
        {
            List<PatientResponse> responses = await _patientService.GetAllPatientsByAccountIdAsync(accountId);

            return Ok(responses);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PatientResponse? response = await _patientService.UpdatePatientAsync(id, request);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            bool deleted = await _patientService.DeletePatientAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent(); // Trả về 204 No Content nếu xóa thành công
        }
        [HttpGet("byphone/{phone}")]
        public async Task<IActionResult> GetPatientsByPhone(string phone)
        {
            try
            {
                var patients = await _patientService.GetPatientsByPhoneAsync(phone);
                if (patients == null || patients.Count == 0)
                {
                    return NotFound("No patients found with the provided phone number.");
                }
                return Ok(patients);
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}