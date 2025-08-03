using System.Collections.Generic;
using System.Threading.Tasks;
using E_Vita_APIs.Models; 
namespace E_Vita.Services
{
    internal class ScheduleServices : BaseApiService
    {
        private const string endpoint = "Schedule";

        public async Task<List<Schedule>> GetAllAsync()
        {
            return await GetAsync<List<Schedule>>(endpoint);
        }

        public async Task<Schedule> GetByIdAsync(int id)
        {
            return await GetAsync<Schedule>($"{endpoint}/{id}");
        }

        public async Task<bool> AddAsync(Schedule scheduale)
        {
            return await PostAsync(endpoint, scheduale);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await base.DeleteAsync($"{endpoint}/{id}");
        }
    }
}


