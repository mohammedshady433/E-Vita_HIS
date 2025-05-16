using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;


namespace E_Vita.Services
{
    public class RadiologyService : BaseApiService
    {

        public async Task<List<Radiology>> GetAllAsync()
        {
            return await GetAsync<List<Radiology>>("Radiology");
        }

        public async Task<Radiology> GetByIdAsync(int id)
        {
            return await GetAsync<Radiology>($"Radiology/{id}");
        }

        public async Task<bool> AddAsync(Radiology radiology)
        {
            return await PostAsync("Radiology", radiology);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"Radiology/{id}");
        }
    }
}
