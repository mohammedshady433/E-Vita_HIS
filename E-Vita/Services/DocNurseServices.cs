using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class DocNurseServices : BaseApiService
    {
        public DocNurseServices():base() { }
        private const string ENDPOINT = "DocNurseServices";
        // Get all doc nurse services
        public async Task<List<DocNurseServices>> GetAllAsync()
        {
            return await GetAsync<List<DocNurseServices>>(ENDPOINT);
        }
        // Get doc nurse service by ID
        public async Task<DocNurseServices> GetByIdAsync(int id)
        {
            return await GetAsync<DocNurseServices>($"{ENDPOINT}/{id}");
        }
        // Create new doc nurse service
        public async Task<bool> CreateAsync(DocNurseServices docNurseService)
        {
            return await PostAsync(ENDPOINT, docNurseService);
        }
        // Update existing doc nurse service
        public async Task<bool> UpdateAsync(int id, DocNurseServices docNurseService)
        {
            return await PostAsync($"{ENDPOINT}/{id}", docNurseService);
        }
        // Delete doc nurse service
        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"{ENDPOINT}/Delete/{id}");
        }

    }
}
