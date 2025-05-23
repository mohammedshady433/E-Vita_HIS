using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Practitioner_Role
    {
        public string Code { get; set; }
        public MedicalSpecialty Specialty { get; set; }
        public Service Service { get; set; }
        public string DayOfWeek { get; set; } // e.g., "Monday"
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int PractitionerId { get; set; }
        [ForeignKey("PractitionerId")]
        [JsonIgnore]
        public Practitioner Practitioner { get; set; } // Navigation property   
    }
}

public enum Service
{
    DoctorIN,
    DoctorOUT,
    Nurse,
    Patient,
    Laboratory,
}
  
        public enum MedicalSpecialty
        {
            AllergyAndImmunology,
            Anesthesiology,
            Cardiology,
            ColonAndRectalSurgery,
            CriticalCareMedicine,
            Dermatology,
            EmergencyMedicine,
            EndocrinologyAndMetabolism,
            FamilyMedicine,
            Gastroenterology,
            GeneralSurgery,
            GeriatricMedicine,
            Hematology,
            HospiceAndPalliativeMedicine,
            InfectiousDisease,
            InternalMedicine,
            MedicalGeneticsAndGenomics,
            Nephrology,
            Neurology,
            Neurosurgery,
            NuclearMedicine,
            ObstetricsAndGynecology,
            Oncology,
            Ophthalmology,
            OrthopedicSurgery,
            Otolaryngology,
            Pathology,
            Pediatrics,
            PhysicalMedicineAndRehabilitation,
            PlasticSurgery,
            Podiatry,
            PreventiveMedicine,
            Psychiatry,
            Pulmonology,
            Radiology,
            Rheumatology,
            SleepMedicine,
            SportsMedicine,
            ThoracicSurgery,
            TransplantSurgery,
            Urology,
            VascularSurgery
        }
    


