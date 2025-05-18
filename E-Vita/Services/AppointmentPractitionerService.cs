using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class AppointmentPractitionerService : BaseApiService
    {
        private const string ApiEndpoint = "AppointmentPractitioner";

        public AppointmentPractitionerService() : base()
        {
        }

        /// <summary>
        /// Gets all appointment-practitioner relationships
        /// </summary>
        public async Task<IEnumerable<AppointmentPractitioner>> GetAllAsync()
        {
            return await GetAsync<IEnumerable<AppointmentPractitioner>>(ApiEndpoint);
        }

        /// <summary>
        /// Gets all appointment-practitioner relationships for a specific practitioner
        /// </summary>
        public async Task<IEnumerable<AppointmentPractitioner>> GetByPractitionerIdAsync(int practitionerId)
        {
            return await GetAsync<IEnumerable<AppointmentPractitioner>>($"{ApiEndpoint}/{practitionerId}");
        }

        /// <summary>
        /// Adds a new appointment-practitioner relationship
        /// </summary>
        public async Task<bool> AddAsync(AppointmentPractitioner appointmentPractitioner)
        {
            return await PostAsync(ApiEndpoint, appointmentPractitioner);
        }

        /// <summary>
        /// Updates an existing appointment-practitioner relationship
        /// </summary>
        public async Task<bool> UpdateAsync(AppointmentPractitioner appointmentPractitioner)
        {
            return await PutAsync(
                $"{ApiEndpoint}/{appointmentPractitioner.AppointmentId}/{appointmentPractitioner.PractitionersId}", 
                appointmentPractitioner);
        }

        /// <summary>
        /// Deletes an appointment-practitioner relationship
        /// </summary>
        public async Task<bool> DeleteAsync(int appointmentId, int practitionerId)
        {
            return await DeleteAsync($"{ApiEndpoint}/{appointmentId}/{practitionerId}");
        }
    }
}
