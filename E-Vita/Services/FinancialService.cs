using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class FinancialService : BaseApiService
    {
        private const string Endpoint = "financial";

        public Task<List<Financial>> GetAllAsync()
        {
            return GetAsync<List<Financial>>(Endpoint);
        }

        public Task<Financial> GetByIdAsync(int id)
        {
            return GetAsync<Financial>($"{Endpoint}/{id}");
        }

        public Task<bool> AddAsync(Financial financial)
        {
            return PostAsync(Endpoint, financial);
        }

        public Task<bool> UpdateAsync(int id, Financial updated)
        {
            return PutAsync($"{Endpoint}/{id}", updated);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}