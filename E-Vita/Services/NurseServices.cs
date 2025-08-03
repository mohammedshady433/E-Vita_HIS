using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class NurseServices : BaseApiService
    {
        private const string Endpoint = "Nurse";
        public NurseServices() : base() { }
        // Get all nurses
        public Task<List<Nurse>> GetAllAsync()
        {
            return GetAsync<List<Nurse>>(Endpoint);
        }
        // Get nurse by ID
        public Task<Nurse> GetByIdAsync(int id)
        {
            return GetAsync<Nurse>($"{Endpoint}/{id}");
        }
        // Add a new nurse
        public Task<bool> AddAsync(Nurse nurse)
        {
            return PostAsync(Endpoint, nurse);
        }
        // Update an existing nurse
        public Task<bool> UpdateAsync(int id, Nurse nurse)
        {
            return PutAsync($"{Endpoint}/{id}", nurse);
        }
        // Delete a nurse
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
}
