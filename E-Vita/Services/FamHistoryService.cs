using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using E_Vita_APIs.Models;


namespace E_Vita.Services
{
    
        public class FamHistoryService : BaseApiService
        {
            private const string Endpoint = "famhistory";
            public Task<List<FamHistory>> GetAllAsync()
            {
                return GetAsync<List<FamHistory>>(Endpoint);
            }
        public Task<FamHistory> GetByIdAsync(int id)
        {
            return GetAsync<FamHistory>($"{Endpoint}/{id}");
        }
        public Task<bool> AddAsync(FamHistory famHistory)
        {
            return PostAsync(Endpoint, famHistory);
        }
        public Task<bool> UpdateAsync(int id, FamHistory updated)
        {
            return PutAsync($"{Endpoint}/{id}", updated);
        }
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
    
}
