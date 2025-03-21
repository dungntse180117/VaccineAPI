﻿using Microsoft.AspNetCore.Mvc;
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
                return BadRequest(ex.Message);
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

            return NoContent(); 
        }

        [HttpGet("byphone/{phone}/{accountId}")]
        public async Task<IActionResult> GetPatientsByPhone(string phone,int accountId )
        {
            try
            {
                var patients = await _patientService.GetPatientsByPhoneAsync(phone, accountId);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }
    }
}