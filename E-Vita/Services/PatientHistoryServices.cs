using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    internal class PatientHistoryServices : BaseApiService
    {
        public PatientHistoryServices() : base() { }
        private const string Endpoint = "PatientHistory";
        // Get all patient histories
        public async Task<List<PatientHistory>> GetAllAsync()
        {
            return await GetAsync<List<PatientHistory>>(Endpoint);
        }
        // Get patient history by ID
        public async Task<PatientHistory> GetByIdAsync(int id)
        {
            return await GetAsync<PatientHistory>($"{Endpoint}/{id}");
        }
        // Create new patient history
        public async Task<bool> CreateAsync(PatientHistory patientHistory)
        {
            return await PostAsync(Endpoint, patientHistory);
        }
        // Update existing patient history
        public async Task<bool> UpdateAsync(int id, PatientHistory patientHistory)
        {
            return await PostAsync($"{Endpoint}/{id}", patientHistory);
        }
        // Delete patient history
        public async Task<bool> DeleteAsync(int id)
        {
            return await PostAsync($"{Endpoint}/Delete/{id}", new { Id = id });
        }

    }
}
