using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita.Models;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    class Observation_vitalService : BaseApiService
    {
        public async Task<List<Observation_Vital>> GetObservation_Vital()
        {
            return await GetAsync<List<Observation_Vital>>("Observation_Vital");
        }
        public async Task<Observation_Vital> GetObservation_Vital(int id)
        {
            return await GetAsync<Observation_Vital>($"Observation_Vital/{id}");
        }
        public async Task<bool> AddObservation_Vital(Observation_Vital observation_vital)
        {
            return await PostAsync("Observation_Vital", observation_vital);
        }
        public async Task<bool> UpdateObservation_Vital(Observation_Vital observation_vital)
        {
            return await PutAsync($"Observation_Vital/{observation_vital.Id}", observation_vital);
        }
        public async Task<bool> DeleteObservation_Vital(int id)
        {
            return await DeleteAsync($"Observation_Vital/{id}");
        }
    }
}
