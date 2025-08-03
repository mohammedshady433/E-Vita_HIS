using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    internal class AttendanceServices : BaseApiService
    {
        public AttendanceServices() :base() { }
        private const string ENDPOINT = "Attendance";
        // Get all attendances
        public async Task<List<Attendance>> GetAllAsync()
        {
            return await GetAsync<List<Attendance>>(ENDPOINT);
        }
        // Get attendance by ID
        public async Task<Attendance> GetByIdAsync(int id)
        {
            return await GetAsync<Attendance>($"{ENDPOINT}/{id}");
        }
        // Create new attendance
        public async Task<bool> CreateAsync(Attendance attendance)
        {
            return await PostAsync(ENDPOINT, attendance);
        }
        // Update existing attendance
        public async Task<bool> UpdateAsync(int id, Attendance attendance)
        {
            return await PostAsync($"{ENDPOINT}/{id}", attendance);
        }
        // Delete attendance
        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"{ENDPOINT}/{id}");
        }
}
