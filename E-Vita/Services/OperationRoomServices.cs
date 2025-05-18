using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    class OperationRoomServices:BaseApiService
    {
        public async Task<List<Operation_Room>> GetOperationRoom()
        {
            return await GetAsync<List<Operation_Room>>("OperationRoom");
        }

        public async Task<Operation_Room> GetOperationRoom(int id)
        {
            return await GetAsync<Operation_Room>($"OperationRoom/{id}");
        }
        public async Task<bool> AddOperationRoom(Operation_Room operationRoom)
        {
            return await PostAsync("OperationRoom", operationRoom);
        }
        public async Task<bool> UpdateOperationRoom(Operation_Room operationRoom)
        {
            return await PutAsync($"OperationRoom/{operationRoom.RoomId}", operationRoom);
        }
        public async Task<bool> DeleteOperationRoom(int id)
        {
            return await DeleteAsync($"OperationRoom/{id}");
        }
    }
}
