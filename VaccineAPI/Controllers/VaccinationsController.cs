using Microsoft.AspNetCore.Mvc;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaccinationsController : ControllerBase
    {
        private readonly IVaccinationService _vaccinationService;

        public VaccinationsController(IVaccinationService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VaccinationResponse>>> GetAllVaccinations()
        {
            var vaccinations = await _vaccinationService.GetAllVaccinations();
            return Ok(vaccinations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VaccinationResponse>> GetVaccinationById(int id)
        {
            var vaccination = await _vaccinationService.GetVaccinationById(id);

            if (vaccination == null)
            {
                return NotFound();
            }

            return Ok(vaccination);
        }

        [HttpPost]
        public async Task<ActionResult<VaccinationResponse>> CreateVaccination([FromBody] VaccinationRequest vaccinationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdVaccination = await _vaccinationService.CreateVaccination(vaccinationRequest);

            if (createdVaccination == null)
            {
                return BadRequest("Failed to create vaccination"); //Hoặc StatusCode(500, "...")
            }

            return CreatedAtAction(nameof(GetVaccinationById), new { id = createdVaccination.VaccinationId }, createdVaccination);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVaccination(int id, [FromBody] VaccinationRequest vaccinationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedVaccination = await _vaccinationService.UpdateVaccination(id, vaccinationRequest);

            if (updatedVaccination == null)
            {
                return NotFound();
            }

            return Ok(updatedVaccination);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaccination(int id)
        {
            bool deleted = await _vaccinationService.DeleteVaccination(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
