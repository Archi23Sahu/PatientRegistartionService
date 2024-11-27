using Microsoft.AspNetCore.JsonPatch;
using PatientRegistartionService.Models;
using System.Threading.Tasks;

namespace PatientRegistartionService.Repository
{
    public interface IPatientRepository
    {
        Task<List<PatientModel>> GetAllPatientsAsync();

        Task<PatientModel> GetPatientByIdAsync(string mrnId);

        Task AddPatientAsync(PatientModel patientModel);

        Task<PatientModel> UpdatePatientAsync(string MedicalRecordNumber, JsonPatchDocument<PatientModel> patientModelPatch);

        Task<(bool success, string message)> DeletePatientAsync(string MedicalRecordNumber);
    }
}
