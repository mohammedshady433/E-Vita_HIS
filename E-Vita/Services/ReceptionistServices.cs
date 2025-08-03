using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class ReceptionistServices : BaseApiService
    {
        private const string Endpoint = "Receptionist";
        public ReceptionistServices() : base() { }
        // Get all receptionists
        public Task<List<Receptionist>> GetAllAsync()
        {
            return GetAsync<List<Receptionist>>(Endpoint);
        }
        // Get receptionist by ID
        public Task<Receptionist> GetByIdAsync(int id)
        {
            return GetAsync<Receptionist>($"{Endpoint}/{id}");
        }
        // Add a new receptionist
        public Task<bool> AddAsync(Receptionist receptionist)
        {
            return PostAsync(Endpoint, receptionist);
        }
        // Update an existing receptionist
        public Task<bool> UpdateAsync(int id, Receptionist receptionist)
        {
            return PostAsync($"{Endpoint}/{id}", receptionist);
        }
        // Delete a receptionist
        public Task<bool> DeleteAsync(int id)
        {
            return PostAsync($"{Endpoint}/Delete/{id}", new { Id = id });
        }
    }
}
