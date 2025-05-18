using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using E_Vita.Models;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class OperationRoomServices : BaseApiService
    {
        private const string ENDPOINT = "OperationRooms";

        public OperationRoomServices() : base()
        {
        }

        // Get all operation rooms
        public async Task<List<Operation_Room>> GetAllAsync()
        {
            try
            {
                var rooms = await GetAsync<List<Operation_Room>>(ENDPOINT);
                return rooms ?? new List<Operation_Room>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading operation rooms: {ex.Message}");
                return new List<Operation_Room>();
            }
        }

        // Get operation room by ID
        public async Task<Operation_Room> GetByIdAsync(int id)
        {
            try
            {
                return await GetAsync<Operation_Room>($"{ENDPOINT}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading operation room {id}: {ex.Message}");
                return null;
            }
        }

        // Create new operation room
        public async Task<bool> CreateAsync(Operation_Room operationRoom)
        {
            try
            {
                return await PostAsync(ENDPOINT, operationRoom);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating operation room: {ex.Message}");
                return false;
            }
        }

        // Update existing operation room
        public async Task<bool> UpdateAsync(int id, Operation_Room operationRoom)
        {
            try
            {
                return await PutAsync($"{ENDPOINT}/{id}", operationRoom);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating operation room {id}: {ex.Message}");
                return false;
            }
        }

        // Delete operation room
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await DeleteAsync($"{ENDPOINT}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting operation room {id}: {ex.Message}");
                return false;
            }
        }
    }
}
