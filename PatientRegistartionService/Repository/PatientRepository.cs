using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using PatientRegistartionService.Data;
using PatientRegistartionService.Models;
namespace PatientRegistartionService.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatientRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PatientModel>> GetAllPatientsAsync()
        {            
            var records = await _context.Patients.ToListAsync();           
            return _mapper.Map<List<PatientModel>>(records);

        }

        public async Task<PatientModel> GetPatientByIdAsync(string mrnId)
        {            

            var records = await _context.Patients.FirstOrDefaultAsync(x=> x.MedicalRecordNumber == mrnId);
            if (records == null)
            {
                return null;
            }
            return _mapper.Map<PatientModel>(records);

        }

        public async Task AddPatientAsync(PatientModel patientModel)
        {
            var existingPatient = await _context.Patients
        .FirstOrDefaultAsync(p => p.MedicalRecordNumber == patientModel.MedicalRecordNumber);

            if (existingPatient != null)
            {
                throw new InvalidOperationException("A patient with this Medical Record Number already exists.");
            }

       
            if (patientModel.DiagnosisName == null)
            {
                patientModel.PhysicianName = null;
                patientModel.DepartmentName = null;
            }
            else
            {
                string diagName = patientModel.DiagnosisName.ToLower();

                if (diagName == "breast cancer" || diagName == "breast" || diagName == "lung cancer" || diagName == "lung")
                {
                    patientModel.PhysicianName = "Dr. Susan Jones";
                    patientModel.DepartmentName = "Department J";
                }
                else
                {
                    patientModel.PhysicianName = "Dr. Ben Smith";
                    patientModel.DepartmentName = "Department S";
                }

            }
            
            
            var patient = _mapper.Map<Patients>(patientModel);
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();             

        }

        public async Task<PatientModel?> UpdatePatientAsync(string MedicalRecordNumber, JsonPatchDocument<PatientModel> patientModelPatch)
        {
            var patientEntity = await _context.Patients.FirstOrDefaultAsync(p => p.MedicalRecordNumber == MedicalRecordNumber);

            if (patientEntity == null)
            {
                return null; // Patient not found
            }

            // Map the Patients entity to the PatientModel DTO
            var patientModel = _mapper.Map<PatientModel>(patientEntity);

            // Apply the patch to the PatientModel
            patientModelPatch.ApplyTo(patientModel);

            if (string.IsNullOrWhiteSpace(patientModel.DiagnosisName))   
            {
                patientModel.DiagnosisName = null;
            }

            if (patientModel.DiagnosisName != null)
            {
                string diagName = patientModel.DiagnosisName.ToLower();
                
                if (diagName == "breast cancer" || diagName == "breast" || diagName == "lung cancer" || diagName == "lung")
                {
                    patientModel.PhysicianName = "Dr. Susan Jones";
                    patientModel.DepartmentName = "Department J";
                }
                else
                {
                    patientModel.PhysicianName = "Dr. Ben Smith";
                    patientModel.DepartmentName = "Department S";
                }
            }

            //Map the updated PatientModel back to the Patients entity
            _mapper.Map(patientModel, patientEntity);


            // Save the changes to the database
            await _context.SaveChangesAsync();

            return patientModel; 
        }


        public async Task<(bool success, string message)> DeletePatientAsync(string MedicalRecordNumber)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.MedicalRecordNumber == MedicalRecordNumber);

            if (patient != null)
            {
                if (patient.DiagnosisName != null)
                {
                    return (false, "Cannot delete patient with an assigned diagnosis name.");

                }
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                return (true, "Patient deleted successfully.");
            }
            return (false, "Patient not found.");
        }
    }
}
