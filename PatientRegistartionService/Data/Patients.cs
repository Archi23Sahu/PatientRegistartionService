namespace PatientRegistartionService.Data
{
    public class Patients
    {
        public int PatientId { get; set; } // Primary key
        public string MedicalRecordNumber { get; set; } // Unique MRN
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmergencyContact { get; set; }
        public string? DiagnosisName { get; set; }
        public string? DepartmentName { get; set; }
        public string? PhysicianName { get; set; }
    }
}
