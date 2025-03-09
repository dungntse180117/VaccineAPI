using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaccinationServicesController : ControllerBase
    {
        private readonly IVaccinationServiceService _vaccinationService;

        public VaccinationServicesController(IVaccinationServiceService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var services = await _vaccinationService.GetAll();
            return Ok(services);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var service = await _vaccinationService.GetById(id);

                if (service == null)
                {
                    return NotFound(); // Trả về 404 Not Found nếu service không tồn tại
                }
                return Ok(service);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi truy xuất dữ liệu từ database");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] VaccinationServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var serviceId = await _vaccinationService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = serviceId }, null);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi tạo vaccination service mới");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] VaccinationServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("ID phải lớn hơn zero.");
            }
            try
            {
                await _vaccinationService.Update(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi cập nhật vaccination service");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID phải lớn hơn zero.");
            }
            try
            {
                await _vaccinationService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi xóa vaccination service");
            }
        }

        [HttpPost("vaccination")]
        public async Task<IActionResult> CreateVaccinationServiceVaccination([FromBody] VaccinationServiceVaccinationRequest request)
        {
            try
            {
                await _vaccinationService.CreateVaccinationServiceVaccination(request);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi tạo liên kết VaccinationService - Vaccination");
            }
        }

        [HttpDelete("vaccination")]
        public async Task<IActionResult> DeleteVaccinationServiceVaccination([FromBody] VaccinationServiceVaccinationRequest request)
        {
            try
            {
                await _vaccinationService.DeleteVaccinationServiceVaccination(request);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi xóa liên kết VaccinationService - Vaccination");
            }
        }

    }
}