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

        public async Task<List<Room>> GetAllAsync()
        {
            return await GetAsync<List<Room>>("Room");
        }

        public async Task<Room> GetByIdAsync(int id)
        {
            return await GetAsync<Room>($"Room/{id}");
        }

        public async Task<bool> AddAsync(Room room)
        {
            return await PostAsync("Room", room);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"Room/{id}");
        }

        public async Task<bool> UpdateAsync(int id, Room updatedRoom)
        {
            return await PutAsync($"Room/{id}", updatedRoom);
        }
    }
}