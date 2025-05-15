using E_Vita.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Vita.Services
{
    public static class PatientService
    {
        // Temporary mock data for demonstration
        private static List<Patient> _mockPatients = new List<Patient>
        {
            new Patient 
            { 
                PatientId = "P001", 
                PatientName = "John Doe", 
                RoomNumber = "101", 
                AdmissionDate = DateTime.Now.AddDays(-5) 
            },
            new Patient 
            { 
                PatientId = "P002", 
                PatientName = "Jane Smith", 
                RoomNumber = "102", 
                AdmissionDate = DateTime.Now.AddDays(-3) 
            }
        };

        public static async Task<List<Patient>> GetPatientsReadyForDischarge()
        {
            // Simulate async operation
            await Task.Delay(100);
            return _mockPatients;
        }

        public static async Task ProcessDischarge(DischargeInfo dischargeInfo)
        {
            // Simulate async operation
            await Task.Delay(100);
            
            // In a real application, this would update the database
            var patient = _mockPatients.Find(p => p.PatientId == dischargeInfo.PatientId);
            if (patient != null)
            {
                _mockPatients.Remove(patient);
            }
        }
    }
} 