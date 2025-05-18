using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class DischargeService : BaseApiService
    {
        private const string ENDPOINT = "Discharge";

        public DischargeService() : base()
        {
        }

        // Get all discharges
        public async Task<List<Discharge>> GetAllAsync()
        {
            return await GetAsync<List<Discharge>>(ENDPOINT);
        }

        // Get discharge by ID
        public async Task<Discharge> GetByIdAsync(int id)
        {
            return await GetAsync<Discharge>($"{ENDPOINT}/{id}");
        }


        // Create new discharge
        public async Task<bool> CreateAsync(Discharge discharge)
        {
            return await PostAsync(ENDPOINT, discharge);
        }

    }
}
