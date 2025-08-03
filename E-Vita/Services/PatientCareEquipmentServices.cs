using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    internal class PatientCareEquipmentServices: BaseApiService
    {
        private const string ENDPOINT = "PatientCareEquipment";
        public PatientCareEquipmentServices() : base() { }
        // Get all patient care equipment
        public async Task<List<PatientCareEquipment>> GetAllAsync()
        {
            return await GetAsync<List<PatientCareEquipment>>(ENDPOINT);
        }
        // Get patient care equipment by ID
        public async Task<PatientCareEquipment> GetByIdAsync(int id)
        {
            return await GetAsync<PatientCareEquipment>($"{ENDPOINT}/{id}");
        }
        // Create new patient care equipment
        public async Task<bool> CreateAsync(PatientCareEquipment equipment)
        {
            return await PostAsync(ENDPOINT, equipment);
        }
        // Update existing patient care equipment
        public async Task<bool> UpdateAsync(int id, PatientCareEquipment equipment)
        {
            return await PostAsync($"{ENDPOINT}/{id}", equipment);
        }
        // Delete patient care equipment
        public async Task<bool> DeleteAsync(int id)
        {
            return await PostAsync($"{ENDPOINT}/Delete/{id}", new { Id = id });
        }
    }
}
