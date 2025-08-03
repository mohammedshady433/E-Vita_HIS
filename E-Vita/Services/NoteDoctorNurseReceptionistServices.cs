using E_Vita_APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Vita.Services
{
    internal class NoteDoctorNurseReceptionistServices : BaseApiService
    {
        private const string Endpoint = "NoteDoctorNurseReceptionist";
        public NoteDoctorNurseReceptionistServices() : base() { }
        // Get all notes
        public Task<List<NoteDoctorNurseReceptionist>> GetAllAsync()
        {
            return GetAsync<List<NoteDoctorNurseReceptionist>>(Endpoint);
        }
        // Get note by ID
        public Task<NoteDoctorNurseReceptionist> GetByIdAsync(int id)
        {
            return GetAsync<NoteDoctorNurseReceptionist>($"{Endpoint}/{id}");
        }
        // Add a new note
        public Task<bool> AddAsync(NoteDoctorNurseReceptionist note)
        {
            return PostAsync(Endpoint, note);
        }
        // Update an existing note
        public Task<bool> UpdateAsync(int id, NoteDoctorNurseReceptionist note)
        {
            return PutAsync($"{Endpoint}/{id}", note);
        }
        // Delete a note
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
