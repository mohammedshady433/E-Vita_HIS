using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class ServiceServices : BaseApiService
    {
        private const string Endpoint = "Service";
        public ServiceServices() : base() { }
        // Get all services
        public Task<List<Service>> GetAllAsync()
        {
            return GetAsync<List<Service>>(Endpoint);
        }
        // Get service by ID
        public Task<Service> GetByIdAsync(int id)
        {
            return GetAsync<Service>($"{Endpoint}/{id}");
        }
        // Add a new service
        public Task<bool> AddAsync(Service service)
        {
            return PostAsync(Endpoint, service);
        }
        // Update an existing service
        public Task<bool> UpdateAsync(int id, Service service)
        {
            return PutAsync($"{Endpoint}/{id}", service);
        }
        // Delete a service
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
