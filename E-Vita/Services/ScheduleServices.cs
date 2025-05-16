using System.Collections.Generic;
using System.Threading.Tasks;
using E_Vita_APIs.Models; 

namespace E_Vita.Services
{
    internal class ScheduleServices : BaseApiService
    {
        private const string endpoint = "schedle";

        public async Task<List<Scheduale>> GetAllAsync()
        {
            return await GetAsync<List<Scheduale>>(endpoint);
        }

        public async Task<Scheduale> GetByIdAsync(int id)
        {
            return await GetAsync<Scheduale>($"{endpoint}/{id}");
        }

        public async Task<bool> AddAsync(Scheduale scheduale)
        {
            return await PostAsync(endpoint, scheduale);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await base.DeleteAsync($"{endpoint}/{id}");
        }
    }
}


