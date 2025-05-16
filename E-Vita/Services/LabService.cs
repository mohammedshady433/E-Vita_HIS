using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Services
{
    public class LabService : BaseApiService
    {
        private const string Endpoint = "lab";

        public Task<List<Lab>> GetAllAsync()
        {
            return GetAsync<List<Lab>>(Endpoint);
        }
        public Task<Lab> GetByIdAsync(int id)
        {
            return GetAsync<Lab>($"{Endpoint}/{id}");
        }
        public Task<bool> AddAsync(Lab lab)
        {
            return PostAsync(Endpoint, lab);
        }
        public Task<bool> UpdateAsync(int id, Lab lab)
        {
            return PutAsync($"{Endpoint}/{id}", lab);
        }
        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
