using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita.Models;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class AppointmentsService : BaseApiService
    {
        private const string ENDPOINT = "Appointment";

        public AppointmentsService() : base()
        {
        }

        // Get all appointments
        public async Task<List<Appointment>> GetAllAsync()
        {
            return await GetAsync<List<Appointment>>(ENDPOINT);
        }

        // Get appointment by ID
        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await GetAsync<Appointment>($"{ENDPOINT}/{id}");
        }

        // Create new appointment
        public async Task<bool> CreateAsync(Appointment appointment)
        {
            return await PostAsync(ENDPOINT, appointment);
        }

        // Update existing appointment
        public async Task<bool> UpdateAsync(int id, Appointment appointment)
        {
            return await PutAsync($"{ENDPOINT}/{id}", appointment);
        }

        // Delete appointment
        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"{ENDPOINT}/{id}");
        }
    }
}
