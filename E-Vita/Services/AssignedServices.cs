using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class AssignedServices : BaseApiService
    {
        private const string ENDPOINT = "Assigned";
        public AssignedServices() : base() { }
        // Get all assigned services
        public async Task<List<Assigned>> GetAllAsync()
        {
            return await GetAsync<List<Assigned>>(ENDPOINT);
        }
        // Get assigned service by ID
        public async Task<Assigned> GetByIdAsync(int id)
        {
            return await GetAsync<Assigned>($"{ENDPOINT}/{id}");
        }
        // Create new assigned service
        public async Task<bool> CreateAsync(Assigned assignedService)
        {
            return await PostAsync(ENDPOINT, assignedService);
        }
        // Update existing assigned service
        public async Task<bool> UpdateAsync(int id, Assigned assignedService)
        {
            return await PostAsync($"{ENDPOINT}/{id}", assignedService);
        }
        // Delete assigned service
        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"{ENDPOINT}/{id}");

        }
}
