using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharmacy_ASP_API.Models.Entities;


namespace PharmaApp.services
{
    public class OrderService : BaseApiService
    {
        private const string Endpoint = "Order";

        public Task<List<Order>> GetAllAsync()
        {
            return GetAsync<List<Order>>(Endpoint);
        }

        public Task<Order> GetByIdAsync(int id)
        {
            return GetAsync<Order>($"{Endpoint}/{id}");
        }

        public Task<bool> AddAsync(Order order)
        {
            return PostAsync(Endpoint, order);
        }

        public Task<bool> UpdateAsync(int id, Order order)
        {
            return PutAsync($"{Endpoint}/{id}", order);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }

}
