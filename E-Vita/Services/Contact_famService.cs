using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class Contact_famService : BaseApiService
    {
        private const string ENDPOINT = "Contact_fam";

        public Contact_famService() : base()
        {
        }

        // Get all family contacts
        public async Task<List<Contact_fam>> GetAllAsync()
        {
            return await GetAsync<List<Contact_fam>>(ENDPOINT);
        }

        // Get family contact by ID
        public async Task<Contact_fam> GetByIdAsync(int id)
        {
            return await GetAsync<Contact_fam>($"{ENDPOINT}/{id}");
        }

        // Create new family contact
        public async Task<bool> CreateAsync(Contact_fam contact)
        {
            return await PostAsync(ENDPOINT, contact);
        }

    }
}
