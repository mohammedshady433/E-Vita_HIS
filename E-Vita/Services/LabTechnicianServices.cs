using E_Vita_APIs.Models;
using PharmaApp.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Vita.Services
{
    internal class LabTechnicianServices : BaseApiService
    {
        private const string Endpoint = "labtechnician";
        public LabTechnicianServices() : base() { }
        public Task<List<Lab_technician>> GetAllAsync()
        {
            return GetAsync<List<Lab_technician>>(Endpoint);
        }
        public Task<Lab_technician> GetByIdAsync(int id)
        {
            return GetAsync<Lab_technician>($"{Endpoint}/{id}");
        }
        public Task<bool> AddAsync(Lab_technician labTechnician)
        {
            return PostAsync(Endpoint, labTechnician);
        }
        public Task<bool> UpdateAsync(int id, Lab_technician labTechnician)
        {
            return PutAsync($"{Endpoint}/{id}", labTechnician);
        }
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
