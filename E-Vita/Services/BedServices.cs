using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class BedServices : BaseApiService
    {
        private const string ENDPOINT = "Bed";

        public BedServices() : base()
        {
        }

        // Get all beds
        public async Task<List<Bed>> GetAllAsync()
        {
            return await GetAsync<List<Bed>>(ENDPOINT);
        }

        // Get bed by ID
        public async Task<Bed> GetByIdAsync(int id)
        {
            return await GetAsync<Bed>($"{ENDPOINT}/{id}");
        }

        // Create new bed
        public async Task<bool> PostAsync(Bed bed)
        {
            return await PostAsync(ENDPOINT, bed);
        }

        // Update existing bed
        public async Task<bool> PutAsync(int id, Bed bed)
        {
            return await PutAsync($"{ENDPOINT}/{id}", bed);
        }

        // Delete bed
        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"{ENDPOINT}/{id}");
        }
    }
}
