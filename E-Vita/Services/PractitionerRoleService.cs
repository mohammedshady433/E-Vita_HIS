using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    class PractitionerRoleService : BaseApiService
    {
        public async Task<List<Practitioner_Role>> GetPractitionerRolesAsync()
        {
            return await GetAsync<List<Practitioner_Role>>("Practitioner_Role");
        }
        public async Task<Practitioner_Role> GetPractitionerRoleByIdAsync(int id)
        {
            return await GetAsync<Practitioner_Role>($"Practitioner_Role/{id}");
        }
        public async Task<bool> AddPractitionerRoleAsync(Practitioner_Role practitionerRole)
        {
            return await PostAsync("Practitioner_Role", practitionerRole);
        }
        public async Task<bool> UpdatePractitionerRoleAsync(Practitioner_Role practitionerRole)
        {
            return await PutAsync($"Practitioner_Role/{practitionerRole.PractitionerId}", practitionerRole);
        }
        public async Task<bool> DeletePractitionerRoleAsync(int id)
        {
            return await DeleteAsync($"Practitioner_Role/{id}");
        }
        
    }
}
