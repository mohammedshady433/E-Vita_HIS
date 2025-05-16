using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita.Models;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    class PatientServices : BaseApiService
    {
        public async Task<List<Patient>> GetPatientsAsync()
        {
            return await GetAsync<List<Patient>>("Patient");
        }
        public async Task<List<Patient>> GetPatientsAsync(int id)
        {
            return await GetAsync<List<Patient>>($"Patient/{id}");
        }
        public async Task<bool> AddPatientAsync(Patient patient)
        {
            return await PostAsync("Patient", patient);
        }
        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            return await PutAsync($"Patient/{patient.ID}", patient);
        }
        public async Task<bool> DeletePatientAsync(int id)
        {
            return await DeleteAsync($"Patient/{id}");
        }
    }
}
