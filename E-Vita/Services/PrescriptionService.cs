using E_Vita_APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Vita.Services
{
    class PrescriptionService : BaseApiService
    {
        public async Task<List<Prescription>> GetPrescriptionsAsync()
        {
            return await GetAsync<List<Prescription>>("Prescription");
        }
        public async Task<List<Prescription>> GetPrescriptionsAsync(int id)
        {
            return await GetAsync<List<Prescription>>($"Prescription/{id}");
        }
        public async Task<bool> AddPrescriptionAsync(Prescription prescription)
        {
            return await PostAsync("Prescription", prescription);
        }
        public async Task<bool> UpdatePrescriptionAsync(Prescription prescription)
        {
            return await PutAsync($"Prescription/{prescription.Id}", prescription);
        }
        public async Task<bool> DeletePrescriptionAsync(int id)
        {
            return await DeleteAsync($"Prescription/{id}");
        }
    }
}
