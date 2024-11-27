using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PatientRegistartionService.Models;
using PatientRegistartionService.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace PatientRegistartionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(IPatientRepository patientRepository, ILogger<PatientsController> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetAllPatients()
        {

            try
            {
                var patients = await _patientRepository.GetAllPatientsAsync();
                if (patients == null || !patients.Any())
                {
                    _logger.LogWarning("No patients found in the database.");
                    return NotFound(new { message = "No patients found." });
                }
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the list of patients.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving patients." });
            }

        }

        [HttpGet("{mrnId}")]
        public async Task<IActionResult> GetPatientById([FromRoute] string mrnId)
        {
            try
            {
                var patient = await _patientRepository.GetPatientByIdAsync(mrnId);
                if (patient == null)
                {
                    return NotFound("Patient not avaliable.");
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving patient {mrnId}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the patient." });
            }

        }

        [HttpPost]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientModel patient)
        {
            try
            {
                if (patient == null)
                {
                    return BadRequest("Patient details are required.");
                }
                await _patientRepository.AddPatientAsync(patient);
                return Ok("Patient registered successfully.");

            }
            catch (InvalidOperationException ex)
            {                
                return Conflict(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while registering a patient.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }

        }


        [HttpPatch("{MedicalRecordNumber}")]
        [Authorize]
        public async Task<IActionResult> UpdatePatientPatch([FromRoute] string MedicalRecordNumber, [FromBody] JsonPatchDocument<PatientModel> patient)
        {
            try
            {
                if (patient == null)
                {
                    return BadRequest("Invalid patient patch document.");
                }

                var result = await _patientRepository.UpdatePatientAsync(MedicalRecordNumber, patient);

                if (result == null)
                {
                    return NotFound("Patient not found");
                }
                else
                {
                    return Ok("Patient updated successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the patient with MRN: {MedicalRecordNumber}", MedicalRecordNumber);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }

        }


        [HttpDelete("{MedicalRecordNumber}")]
        [Authorize]
        public async Task<IActionResult> DeletePatient([FromRoute] string MedicalRecordNumber)
        {
            try
            {
                var (success, message) = await _patientRepository.DeletePatientAsync(MedicalRecordNumber);

                if (success)
                {
                    return Ok(message);
                }
                else
                {
                    return BadRequest(message);
                }

            }
            catch(DBConcurrencyException e)
            {
                _logger.LogError($"{e}");
                return StatusCode(StatusCodes.Status502BadGateway, new { message = "Database issue occurred." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting patient {MedicalRecordNumber}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the patient." });

            }

        }
    }
}

