using E_Vita_APIs.Models; 
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
namespace E_Vita.Services
{
    internal class WardroundServices : BaseApiService
    {
        private const string endpoint = "wardround"; 

        
        public async Task<List<WardRound>> GetAllAsync()
        {
            return await GetAsync<List<WardRound>>(endpoint);
        }

        
        public async Task<WardRound> GetByIdAsync(int id)
        {
            return await GetAsync<WardRound>($"{endpoint}/{id}");
        }

        
        public async Task<bool> AddAsync(WardRound wardRound)
        {
            return await PostAsync(endpoint, wardRound);
        }
        public async Task<E_Vita_APIs.Models.WardRound> AddAsyncreturnID(E_Vita_APIs.Models.WardRound wardRound)
        {
            return await PostAsyncWithResponse<E_Vita_APIs.Models.WardRound>(endpoint, wardRound);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await base.DeleteAsync($"{endpoint}/{id}");
        }
    }
}

