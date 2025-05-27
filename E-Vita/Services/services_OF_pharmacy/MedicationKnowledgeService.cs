using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharmacy_ASP_API.Models.Entities;

namespace PharmaApp.services
{
    public class MedicationKnowledgeService : BaseApiService
    {
        private const string Endpoint = "MedicationKnowledge";

        public Task<List<MedicationKnowledge>> GetAllAsync()
        {
            return GetAsync<List<MedicationKnowledge>>(Endpoint);
        }

        public Task<MedicationKnowledge> GetByIdAsync(string id)
        {
            return GetAsync<MedicationKnowledge>($"{Endpoint}/{id}");
        }

        public Task<bool> AddAsync(MedicationKnowledge knowledge)
        {
            return PostAsync(Endpoint, knowledge);
        }

        public Task<bool> UpdateAsync(string id, MedicationKnowledge knowledge)
        {
            return PutAsync($"{Endpoint}/{id}", knowledge);
        }

        public Task<bool> DeleteAsync(string id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }

        
    }
}




