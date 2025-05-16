using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using E_Vita_APIs.Models;


namespace E_Vita.Services
{
    public class EncounterService : BaseApiService
    {
        private const string Endpoint = "encounter";
        public async Task<List<Encounter>> GetEncountersAsync()
        {
            return await GetAsync<List<Encounter>>(Endpoint);
        }
        public async Task<Encounter> GetByIdAsync(int id)
        {
            return await GetAsync<Encounter>($"{Endpoint}/{id}");
        }
        public async Task<bool> CreateAsync(Encounter encounter)
        {
            return await PostAsync(Endpoint, encounter);
        }
        public async Task<bool> UpdateAsync(int id, Encounter encounter)
        {
            return await PutAsync($"{Endpoint}/{id}", encounter);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"{Endpoint}/{id}");
        }


    }
}
