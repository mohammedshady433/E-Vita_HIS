using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharmacy_ASP_API.Models.Entities;


namespace PharmaApp.services
{
    public class PatientService : BaseApiService
    {
        private const string Endpoint = "Patient";

        public Task<List<Patient>> GetAllAsync()
        {
            return GetAsync<List<Patient>>(Endpoint);
        }

        public Task<Patient> GetByIdAsync(int id)
        {
            return GetAsync<Patient>($"{Endpoint}/{id}");
        }

        public Task<bool> AddAsync(Patient patient)
        {
            return PostAsync(Endpoint, patient);
        }

        public Task<bool> UpdateAsync(int id, Patient patient)
        {
            return PutAsync($"{Endpoint}/{id}", patient);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
