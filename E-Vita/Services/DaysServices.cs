using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class DaysServices : BaseApiService
    {
        public DaysServices() : base() { }
        private const string ENDPOINT = "Days";
        // Get all days
        public async Task<List<Days>> GetAllAsync()
        {
            return await GetAsync<List<Days>>(ENDPOINT);
        }
        // Get day by ID
        public async Task<Days> GetByIdAsync(int id)
        {
            return await GetAsync<Days>($"{ENDPOINT}/{id}");
        }
        // Create new day
        public async Task<bool> CreateAsync(Days day)
        {
            return await PostAsync(ENDPOINT, day);
        }
        // Update existing day
        public async Task<bool> UpdateAsync(int id, Days day)
        {
            return await PostAsync($"{ENDPOINT}/{id}", day);
        }
        // Delete day
        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"{ENDPOINT}/{id}");
        }
}
