
using System.ComponentModel.DataAnnotations;

namespace PatientRegistartionService.Models
{
    public class PatientModel
    {
        public int PatientId { get; set; } // Primary key

        [Required]
        public string MedicalRecordNumber { get; set; } // Unique MRN

        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string EmergencyContact { get; set; }

        
        public string? DiagnosisName { get; set; }
        public string? DepartmentName { get; set; }
        public string? PhysicianName { get; set; }
    }
}
