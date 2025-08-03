using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class DoctorNursePatientServices: BaseApiService
    {
        public DoctorNursePatientServices() : base() { }
        private const string ENDPOINT = "DoctorNursePatient";
        // Get all doctor-nurse-patient services
        public async Task<List<DoctorNursePatient>> GetAllAsync()
        {
            return await GetAsync<List<DoctorNursePatient>>(ENDPOINT);
        }
        // Get doctor-nurse-patient service by ID
        public async Task<DoctorNursePatient> GetByIdAsync(int id)
        {
            return await GetAsync<DoctorNursePatient>($"{ENDPOINT}/{id}");
        }
        // Create new doctor-nurse-patient service
        public async Task<bool> CreateAsync(DoctorNursePatient doctorNursePatient)
        {
            return await PostAsync(ENDPOINT, doctorNursePatient);
        }
        // Update existing doctor-nurse-patient service
        public async Task<bool> UpdateAsync(int id, DoctorNursePatient doctorNursePatient)
        {
            return await PostAsync($"{ENDPOINT}/{id}", doctorNursePatient);
        }
        // Delete doctor-nurse-patient service
        public async Task<bool> DeleteAsync(int id)
        {
            return await PostAsync($"{ENDPOINT}/Delete/{id}", new { Id = id });
        }
    }
}
