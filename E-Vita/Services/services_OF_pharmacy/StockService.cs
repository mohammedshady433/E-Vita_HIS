using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharmacy_ASP_API.Models.Entities;


namespace PharmaApp.services
{
    public class StockService : BaseApiService
    {
        private const string Endpoint = "Stock";

        public Task<List<Stock>> GetAllAsync()
        {
            return GetAsync<List<Stock>>(Endpoint);
        }

        public Task<Stock> GetByIdAsync(int id)
        {
            return GetAsync<Stock>($"{Endpoint}/{id}");
        }

        public Task<bool> AddAsync(Stock stock)
        {
            return PostAsync(Endpoint, stock);
        }

        public Task<bool> UpdateAsync(int id, Stock stock)
        {
            return PutAsync($"{Endpoint}/{id}", stock);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
