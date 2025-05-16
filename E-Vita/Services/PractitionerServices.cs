using E_Vita_APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Vita.Services
{
    class PractitionerServices : BaseApiService
    {
        public async Task<List<Practitioner>> GetPractitionersAsync()
        {
            return await GetAsync<List<Practitioner>>("Practitioner");
        }
        public async Task<Practitioner> GetPractitionerByIdAsync(int id)
        {
            return await GetAsync<Practitioner>($"Practitioner/{id}");
        }
        public async Task<bool> AddPractitionerAsync(Practitioner practitioner)
        {
            return await PostAsync("Practitioner", practitioner);
        }
        public async Task<bool> UpdatePractitionerAsync(Practitioner practitioner)
        {
            return await PutAsync($"Practitioner/{practitioner.Id}", practitioner);
        }
        public async Task<bool> DeletePractitionerAsync(int id)
        {
            return await DeleteAsync($"Practitioner/{id}");
        }
    }
}
