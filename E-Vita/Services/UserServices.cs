using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class UserServices : BaseApiService
    {
        private const string Endpoint = "User";
        public UserServices() : base() { }
        // Get all users
        public Task<List<User>> GetAllAsync()
        {
            return GetAsync<List<User>>(Endpoint);
        }
        // Get user by ID
        public Task<User> GetByIdAsync(int id)
        {
            return GetAsync<User>($"{Endpoint}/{id}");
        }
        // Add a new user
        public Task<bool> AddAsync(User user)
        {
            return PostAsync(Endpoint, user);
        }
        // Update an existing user
        public Task<bool> UpdateAsync(int id, User user)
        {
            return PutAsync($"{Endpoint}/{id}", user);
        }
        // Delete a user
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
