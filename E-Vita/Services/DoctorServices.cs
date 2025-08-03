using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class DoctorServices : BaseApiService
    {
        public DoctorServices() : base() { }
        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            return await GetAsync<List<Doctor>>("doctors");
        }
        public async Task<bool> AddDoctorAsync(Doctor doctor)
        {
            return await PostAsync("doctors", doctor);
        }
        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            return await PostAsync("doctors/update", doctor);
        }
        public async Task<bool> DeleteDoctorAsync(int id)
        {
            return await PostAsync("doctors/delete", new { Id = id });
        }
        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            return await GetAsync<Doctor>($"doctors/{id}");
        }

    }
}
