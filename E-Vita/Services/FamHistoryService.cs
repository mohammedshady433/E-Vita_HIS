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
            public Task<List<FamilyHistory>> GetAllAsync()
            {
                return GetAsync<List<FamilyHistory>>(Endpoint);
            }
        public Task<FamilyHistory> GetByIdAsync(int id)
        {
            return GetAsync<FamilyHistory>($"{Endpoint}/{id}");
        }
        public Task<bool> AddAsync(FamilyHistory famHistory)
        {
            return PostAsync(Endpoint, famHistory);
        }
        public Task<bool> UpdateAsync(int id, FamilyHistory updated)
        {
            return PutAsync($"{Endpoint}/{id}", updated);
        }
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
    
}
