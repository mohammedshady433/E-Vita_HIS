using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class Rad_technicianServices : BaseApiService
    {
        private const string Endpoint = "Rad_technician";
        public Rad_technicianServices() : base() { }
        // Get all radiology technicians
        public Task<List<Rad_technician>> GetAllAsync()
        {
            return GetAsync<List<Rad_technician>>(Endpoint);
        }
        // Get radiology technician by ID
        public Task<Rad_technician> GetByIdAsync(int id)
        {
            return GetAsync<Rad_technician>($"{Endpoint}/{id}");
        }
        // Add a new radiology technician
        public Task<bool> AddAsync(Rad_technician technician)
        {
            return PostAsync(Endpoint, technician);
        }
        // Update an existing radiology technician
        public Task<bool> UpdateAsync(int id, Rad_technician technician)
        {
            return PutAsync($"{Endpoint}/{id}", technician);
        }
        // Delete a radiology technician
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
