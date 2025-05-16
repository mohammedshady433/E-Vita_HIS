using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class QuantityService : BaseApiService
    {

        public async Task<List<Quantity>> GetAllAsync()
        {
            return await GetAsync<List<Quantity>>("Quantity");
        }

        public async Task<Quantity> GetByIdAsync(int id)
        {
            return await GetAsync<Quantity>($"Quantity/{id}");
        }

        public async Task<bool> AddAsync(Quantity quantity)
        {
            return await PostAsync("Quantity", quantity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"Quantity/{id}");
        }
    }
}
