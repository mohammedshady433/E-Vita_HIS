using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita.Models;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    class MedicationServices :BaseApiService
    {
        public async Task<List<Medication>> GetMedications()
        {
            return await GetAsync<List<Medication>>("Medications");
        }
        public async Task<Medication> GetMedication(int id)
        {
            return await GetAsync<Medication>($"Medications/{id}");
        }
        public async Task<bool> AddMedication(Medication medication)
        {
            return await PostAsync("Medications", medication);
        }

        public async Task<bool> UpdateMedication(Medication medication )
        {
            return await PutAsync($"Medications/{medication.Id}", medication);
        }
        public async Task<bool> DeleteMedication(int id)
        {
            return await DeleteAsync($"Medications/{id}");
        }
    }
}
