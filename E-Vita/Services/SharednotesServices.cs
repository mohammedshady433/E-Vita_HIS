using System.Collections.Generic;
using System.Threading.Tasks;
using E_Vita_APIs.Models; 
namespace E_Vita.Services
{
    internal class SharednotesServices : BaseApiService
    {
        private const string endpoint = "sharednotes"; 

        
        public async Task<List<SharedNote>> GetAllAsync()
        {
            return await GetAsync<List<SharedNote>>(endpoint);
        }

        
        public async Task<SharedNote> GetByIdAsync(int id)
        {
            return await GetAsync<SharedNote>($"{endpoint}/{id}");
        }

       
        public async Task<bool> AddAsync(SharedNote sharedNote)
        {
            return await PostAsync(endpoint, sharedNote);
        }

        
        public async Task<bool> DeleteAsync(int id)
        {
            return await base.DeleteAsync($"{endpoint}/{id}");
        }

        
        public async Task<List<SharedNote>> GetByPractitionerIdAsync(int practitionerId)
        {
            return await GetAsync<List<SharedNote>>($"{endpoint}/practitioner/{practitionerId}");
        }
    }
}

