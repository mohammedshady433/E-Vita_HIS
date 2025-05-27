using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharmacy_ASP_API.Models.Entities;
using PharmaApp.services;

namespace PharmaApp.Services
{
    public class MedicationRequestService : BaseApiService
    {
        private const string Endpoint = "MedicationRequest";
        private const string MedicationEndpoint = "MedicationKnowledge";

        public async Task<List<MedicationRequest>> GetMedicationRequestsAsync()
        {
            return await GetAsync<List<MedicationRequest>>(Endpoint);
        }

        public async Task<MedicationRequest> GetMedicationRequestByIdAsync(string requestId)
        {
            var request = await GetAsync<MedicationRequest>($"{Endpoint}/{requestId}");
            if (request != null)
            {
                request.MedicationKnowledge = await GetAsync<MedicationKnowledge>($"{MedicationEndpoint}/{request.MedicationId}");
            }
            return request;
        }

        public async Task<MedicationKnowledge> GetMedicationKnowledgeByIdAsync(string medicationId)
        {
            return await GetAsync<MedicationKnowledge>($"{MedicationEndpoint}/{medicationId}");
        }

        public async Task<bool> CreateMedicationRequestAsync(MedicationRequest request)
        {
           return await PostAsync(Endpoint, request);
        }

        public async Task<bool> UpdateMedicationRequestAsync(string requestId, MedicationRequest request)
        {
            return await PutAsync($"{Endpoint}/{requestId}", request);
        }

        public async Task<bool> DeleteMedicationRequestAsync(string requestId)
        {
            return await DeleteAsync($"{Endpoint}/{requestId}");
        }
    }
}
