using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class ShareServices : BaseApiService
    {
        public ShareServices():base() { }
        private const string Endpoint = "Share";
        // Get all shares
        public Task<List<E_Vita_APIs.Models.Share>> GetAllAsync()
        {
            return GetAsync<List<E_Vita_APIs.Models.Share>>(Endpoint);
        }
        // Get share by ID
        public Task<E_Vita_APIs.Models.Share> GetByIdAsync(int id)
        {
            return GetAsync<E_Vita_APIs.Models.Share>($"{Endpoint}/{id}");
        }
        // Add a new share
        public Task<bool> AddAsync(E_Vita_APIs.Models.Share share)
        {
            return PostAsync(Endpoint, share);
        }
        // Update an existing share
        public Task<bool> UpdateAsync(int id, E_Vita_APIs.Models.Share share)
        {
            return PutAsync($"{Endpoint}/{id}", share);
        }
        // Delete a share
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
