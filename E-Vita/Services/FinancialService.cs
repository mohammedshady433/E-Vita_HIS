using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
using Microsoft.VisualBasic;

namespace E_Vita.Services
{
    public class FinancialService : BaseApiService
    {
        private const string Endpoint = "financial";

        public Task<List<Finance>> GetAllAsync()
        {
            return GetAsync<List<Finance>>(Endpoint);
        }

        public Task<Finance> GetByIdAsync(int id)
        {
            return GetAsync<Finance>($"{Endpoint}/{id}");
        }

        public Task<bool> AddAsync(Finance financial)
        {
            return PostAsync(Endpoint, financial);
        }

        public Task<bool> UpdateAsync(int id, Finance updated)
        {
            return PutAsync($"{Endpoint}/{id}", updated);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}