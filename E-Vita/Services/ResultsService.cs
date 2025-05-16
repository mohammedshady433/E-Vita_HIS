using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;


namespace E_Vita.Services
{
    public class ResultsService : BaseApiService
    {

        public async Task<List<Results>> GetAllAsync()
        {
            return await GetAsync<List<Results>>("Results");
        }

        public async Task<Results> GetByIdAsync(int id)
        {
            return await GetAsync<Results>($"Results/{id}");
        }

        public async Task<bool> AddAsync(Results results)
        {
            return await PostAsync("Results", results);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"Results/{id}");
        }

        public async Task<bool> UpdateAsync(int id, Results updatedResults)
        {
            return await PutAsync($"Results/{id}", updatedResults);
        }
    }
}
