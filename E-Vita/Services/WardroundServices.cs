using System.Collections.Generic;
using System.Threading.Tasks;
using E_Vita_APIs.Models; 
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

        
        public async Task<bool> DeleteAsync(int id)
        {
            return await base.DeleteAsync($"{endpoint}/{id}");
        }
    }
}

