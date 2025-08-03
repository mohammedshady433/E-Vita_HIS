using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class RoomService : BaseApiService
    {

        public async Task<List<Rooms>> GetAllAsync()
        {
            return await GetAsync<List<Rooms>>("Rooms");
        }

        public async Task<Rooms> GetByIdAsync(int id)
        {
            return await GetAsync<Rooms>($"Rooms/{id}");
        }

        public async Task<bool> AddAsync(Rooms room)
        {
            return await PostAsync("Rooms", room);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"Rooms/{id}");
        }

        public async Task<bool> UpdateAsync(int id, Rooms updatedRoom)
        {
            return await PutAsync($"Rooms/{id}", updatedRoom);
        }
    }
}