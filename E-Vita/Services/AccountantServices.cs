using E_Vita_APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Vita.Services
{
    internal class AccountantServices : BaseApiService
    {
        private const string ENDPOINT = "Accountant";
        public AccountantServices() : base()
        {
        }
        // Get all accountants
        public async Task<List<Accountant>> GetAllAsync()
        {
            return await GetAsync<List<Accountant>>(ENDPOINT);
        }
        // Get accountant by ID
        public async Task<Accountant> GetByIdAsync(int id)
        {
            return await GetAsync<Accountant>($"{ENDPOINT}/{id}");
        }
        // Create new accountant
        public async Task<bool> CreateAsync(Accountant accountant)
        {
            return await PostAsync(ENDPOINT, accountant);
        }
        // Update existing accountant
        public async Task<bool> UpdateAsync(int id, Accountant accountant)
        {
            return await PostAsync($"{ENDPOINT}/{id}", accountant);
        }
        // Delete accountant
        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"{ENDPOINT}/Delete/{id}");
        }

    }
}
